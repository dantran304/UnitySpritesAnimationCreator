using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "AssetCreator/AnimCreator", fileName = "AnimCreator")]
public class AnimationCreator : ScriptableObject
{
    [Space]
    [Header("Đường dẫn đến folder")]
    public string folderPath0 = "";

    [Space]
    [Header("Folder chứa anim. Ví dụ: troop_2000_frame")]
    public string folderPath = "";

    [Space]
    [Header("Có cần tạo animator override hok?")]
    public bool createAnimOverride;

    [Space]
    [Header("Tên các folder con chứa sprite, có bao nhiêu folder sẽ tạo bấy nhiêu file clip")]
    public List<AnimOnFolder> listFolderName = new List<AnimOnFolder>();

    public void CreateAnimation()
    {
        // Creates the controller
        var animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath
            (string.Format("Assets/Resources/{0}/{1}/{2}.controller", folderPath0, folderPath, folderPath));

        if (createAnimOverride)
        {
            AnimatorOverrideController animOverrideController = new AnimatorOverrideController();
            AssetDatabase.CreateAsset(animOverrideController, string.Format("Assets/Resources/{0}/{1}/{2}.overrideController", folderPath0, folderPath, folderPath));
        }

        for (int i = 0; i < listFolderName.Count; i++)
        {
            AnimationClip anim = new AnimationClip();
            anim.frameRate = listFolderName[i].frameRate;
            var animName = listFolderName[i].animFolderName;

            if (listFolderName[i].needOverride)
            {
                AssetDatabase.CreateAsset(anim, string.Format("Assets/Resources/{0}/{1}/{2}.anim", folderPath0, folderPath, animName));

                var spritesPath = string.Format("{0}/{0}/{1}/", folderPath0, folderPath, animName);

                //var cardTexture = (Texture2D).LoadAssetAtPath(fullPath, typeof(Texture2D));

                EditorCurveBinding spriteBinding = new EditorCurveBinding();
                spriteBinding.type = typeof(SpriteRenderer);
                spriteBinding.path = "";
                spriteBinding.propertyName = "m_Sprite";

                //Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(spritesPath);
                Sprite[] sprites = Resources.LoadAll<Sprite>(spritesPath);
                ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprites.Length];

                for (int j = 0; j < (sprites.Length); j++)
                {
                    spriteKeyFrames[j] = new ObjectReferenceKeyframe();
                    spriteKeyFrames[j].time = (float)j / sprites.Length;
                    spriteKeyFrames[j].value = (Sprite)sprites[j];
                }
                AnimationUtility.SetObjectReferenceCurve(anim, spriteBinding, spriteKeyFrames);
            }
        }
    }
}

[System.Serializable]
public class AnimOnFolder
{
    public string animFolderName;
    public int frameRate;
    public bool needOverride;
}
