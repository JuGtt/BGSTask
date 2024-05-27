using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Animation Creator")]
public class AnimationCreator : ScriptableObject
{
    #region Serialized Fields
    [SerializeField, Tooltip("The Sprite Sheet that will be generated animations from.")]
    private Texture2D _spriteSheet;
    [SerializeField, Tooltip("The size of each sprite cell.")]
    private Vector2Int _cellSize = new Vector2Int(64, 64);
    [SerializeField, Tooltip("The Sample Rate of the generated animations.")]
    private float _frameRate = 12f;
    [SerializeField, Tooltip("The prefix that all animations will be named after.")]
    private string _animationPrefix = "Anim_";
    [SerializeField, Tooltip("The path where the animations will be saved.")]
    private string _savePath = "Assets/Animations/";
    #endregion

    #region Private Fields
    private int _spriteSheetWidth;
    private int _spriteSheetHeight;
    private Sprite[] _sprites;
    private Dictionary<string, List<Vector2Int>> _animationCells;
    #endregion

    #region Public Methods
    public void CreateAllAnimations()
    {
        Initialize();

        foreach (KeyValuePair<string, List<Vector2Int>> animation in _animationCells)
        {
            AnimationClip clip = new AnimationClip();
            clip.frameRate = _frameRate;

            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.type = typeof(SpriteRenderer);
            curveBinding.path = "";
            curveBinding.propertyName = "m_Sprite";

            ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[animation.Value.Count];
            for (int i = 0; i < animation.Value.Count; i++)
            {
                keyFrames[i] = new ObjectReferenceKeyframe();
                keyFrames[i].time = i * (1.0f / clip.frameRate);
                // Load the sprite based on the cell index from the sprite sheet
                Sprite sprite = LoadSpriteFromSheet(animation.Value[i]);
                keyFrames[i].value = sprite;
            }

            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);

            string clipName = animation.Key;
            AssetDatabase.CreateAsset(clip, _savePath + clipName + ".anim");
        }

        AssetDatabase.SaveAssets();
    }
    #endregion

    #region Private Methods
    private void Initialize()
    {
        string path = AssetDatabase.GetAssetPath(_spriteSheet);
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        _sprites = assets.OfType<Sprite>().ToArray();

        AnimationMapper();

        _spriteSheetWidth = _spriteSheet.width;
        _spriteSheetHeight = _spriteSheet.height;

    }
    private void AnimationMapper()
    {
        _animationCells = new Dictionary<string, List<Vector2Int>>();

        string[] animations = { "Idle", "Walk", "Run" };
        string[] directions = { "Down", "Up", "Right", "Left" };

        foreach (string animation in animations)
        {
            int cellX = 0;
            int cellY = GetInitialCellY(animation);

            foreach (string direction in directions)
            {
                List<Vector2Int> cells = new List<Vector2Int>();
                cellX = 0;
                GetAnimationCells(new Vector2Int(cellX, cellY), animation, ref cells);
                cellY += _cellSize.y;

                string animName = _animationPrefix + animation + "_" + direction;
                _animationCells.Add(animName, cells);
            }
        }
    }

    private int GetInitialCellY(string animation)
    {
        if (animation == "Walk" || animation == "Run")
        {
            return _cellSize.y * 4;
        }
        return 0;
    }

    private void GetAnimationCells(Vector2Int initialCell, string animation, ref List<Vector2Int> cells)
    {
        int x = initialCell.x;
        int y = initialCell.y;

        if (animation == "Idle")
        {
            cells.Add(new Vector2Int(x, y));
        }
        else if (animation == "Walk")
        {
            for (int i = 0; i < 6; i++)
            {
                cells.Add(new Vector2Int(x, y));
                x += _cellSize.x;
            }
        }
        else if (animation == "Run")
        {
            for (int i = 0; i < 6; i++)
            {
                if (i == 2)
                {
                    int newX = _cellSize.x * 6;
                    cells.Add(new Vector2Int(newX, y));
                }
                else if (i == 5)
                {
                    int newX = _cellSize.x * 7;
                    cells.Add(new Vector2Int(newX, y));
                }
                else
                    cells.Add(new Vector2Int(x, y));

                x += _cellSize.x;
            }
        }
    }

    private Sprite LoadSpriteFromSheet(Vector2Int cellIndex)
    {
        // Calculate index in the _sprites array based on the cell index
        int spritesPerRow = _spriteSheetWidth / 64;
        int index = (cellIndex.y / 64) * spritesPerRow + (cellIndex.x / 64);

        // Ensure the index is within bounds
        if (index >= 0 && index < _sprites.Length)
        {
            return _sprites[index];
        }
        else
        {
            Debug.LogError("Invalid cell index: " + cellIndex);
            return null;
        }
    }
    #endregion
}