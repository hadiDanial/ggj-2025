using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor;


public class SpriteImporter : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        Debug.Log("asset path: " + assetPath);
        TextureImporter textureImporter = (TextureImporter)assetImporter;

        if (assetPath.EndsWith("_ex.png")) //Exceptions. Add your exceptions here!
        {
            //Detail exceptions here
        }
        else if (!assetPath.Contains("AmplifyColor") && 
            !assetPath.Contains("AmplifyBloom") && 
            !assetPath.Contains("LUT") && !assetPath.Contains("Lut") && !assetPath.EndsWith(".exr")) //This is for LUT textures used in Color Correction (Post Processing effect)
        {
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.filterMode = FilterMode.Point;
            textureImporter.spritePixelsPerUnit = 512;
        }
        AssetDatabase.WriteImportSettingsIfDirty(assetPath);
    }
}