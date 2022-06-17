﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.EssentialKit.Editor
{
    public class EssentialKitEditorUtility
    {
        #region Constants

        private     const   string      kDefaultSettingsAssetOldPath    = "Assets/Plugins/VoxelBusters/EssentialKit/Resources/EssentialKitSettings.asset";

        #endregion

        #region Public methods

        public static void ImportEssentialResources()
        {
            var     targetPackages  = new string[]
            {
                $"{CoreLibrarySettings.Package.GetPackageResourcesPath()}/PackageResources/Essentials.unitypackage",
                $"{EssentialKitSettings.Package.GetPackageResourcesPath()}/PackageResources/Essentials.unitypackage",
            };
            ImportPackages(targetPackages);
        }

        public static bool CanMigratePackagesToUPM()
        {
            return EssentialKitSettings.Package.IsInstalledWithinAssets();
        }

        public static void MigratePackagesToUPM()
        {
            CoreLibrarySettings.Package.MigrateToUPM();
            EssentialKitSettings.Package.MigrateToUPM();
        }

        /*
        public static void ImportExtraResources()
        {
            var     targetPackages  = new string[]
            {
                $"{EssentialKitSettings.Package.GetPackageResourcesPath()}/PackageResources/Extras.unitypackage",
            };
            ImportPackages(targetPackages);
        }
        */

        #endregion

        #region Private methods

        private static void RegisterForImportPackageCallbacks()
        {
            AssetDatabase.importPackageStarted     += OnImportPackageStarted;
            AssetDatabase.importPackageCompleted   += OnImportPackageCompleted;
            AssetDatabase.importPackageCancelled   += OnImportPackageCancelled;
            AssetDatabase.importPackageFailed      += OnImportPackageFailed;
        }

        private static void UnregisterFromImportPackageCallbacks()
        {
            AssetDatabase.importPackageStarted     -= OnImportPackageStarted;
            AssetDatabase.importPackageCompleted   -= OnImportPackageCompleted;
            AssetDatabase.importPackageCancelled   -= OnImportPackageCancelled;
            AssetDatabase.importPackageFailed      -= OnImportPackageFailed;
        }

        private static void ImportPackages(string[] packages)
        {
            RegisterForImportPackageCallbacks();
            foreach (var package in packages)
            {
                AssetDatabase.ImportPackage(package, false);
            }
            UnregisterFromImportPackageCallbacks();
        }

        private static bool IsFileStructureOutdated()
        {
            return IOServices.FileExists(kDefaultSettingsAssetOldPath);
        }

        private static void MigrateToNewFileStructure()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            AssetDatabase.MoveAsset(kDefaultSettingsAssetOldPath, EssentialKitSettings.DefaultSettingsAssetPath);
        }

        #endregion

        #region Callback methods

        [InitializeOnLoadMethod]
        private static void PostProcessPackage()
        {
            EditorApplication.delayCall += () =>
            {
                if (IsFileStructureOutdated())
                {
                    MigrateToNewFileStructure();
                }
            };
        }

        private static void OnImportPackageStarted(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Started importing package: {packageName}");
        }

        private static void OnImportPackageCompleted(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Imported package: {packageName}");
        }

        private static void OnImportPackageCancelled(string packageName)
        {
            Debug.Log($"[UnityPackageUtility] Cancelled the import of package: {packageName}");
        }

        private static void OnImportPackageFailed(string packageName, string errorMessage)
        {
            Debug.Log($"[UnityPackageUtility] Failed importing package: {packageName} with error: {errorMessage}");
        }

        #endregion
    }
}