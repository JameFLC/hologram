using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;
public class ImportManager : MonoBehaviour
{
    [SerializeField] private Transform holograms;



    public void ImportScene()
    {
       
        StartCoroutine(ShowLoadDialog());
    }

    private void ClearHolograms()
    {
        foreach (Transform item in holograms)
        {
            Destroy(item.gameObject);
        }
    }

    IEnumerator ShowLoadDialog()
    {
        
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Importer La Scene", "Load");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            // Or, copy the first file to persistentDataPath
            string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);

            ClearHolograms();

            StartCoroutine(LoadBundleFromFileSystem(destinationPath));
        }
    }

    private void Armagedon(Transform obj, int currentRecursion)
    {
        int MaxRecurtion = 100;
        if (currentRecursion < MaxRecurtion)
        {
            foreach (Transform item in obj)
            {
                Armagedon(item, currentRecursion++);
            }
            else
            {
                Debug.LogWarning("Armagedon Recurtion level greater than " + MaxRecurtion);
            }
        }
        

    }
    IEnumerator LoadBundleFromFileSystem(string filePath)
    {


        //Load "animals" AssetBundle
        var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
        yield return assetBundleCreateRequest;

        AssetBundle bundle = assetBundleCreateRequest.assetBundle;


        string rootAssetPath = bundle.GetAllAssetNames()[0];
        
        GameObject obj = Instantiate(bundle.LoadAsset(rootAssetPath) as GameObject, holograms);
    }
}
