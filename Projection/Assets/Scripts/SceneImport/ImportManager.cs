using System.Collections;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;
public class ImportManager : MonoBehaviour
{
    [SerializeField] private Transform holograms;
    [SerializeField] MocapManager moCapManager;
    [SerializeField] LightManager lightManager;
    [SerializeField] SetupUI setupUI;
    private string hologramFilePath;
    private float importScale = 1;

    public void ImportScene()
    {
        
        StartCoroutine(ShowLoadDialog());
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
            hologramFilePath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], hologramFilePath);


            Debug.Log("Hologram File Path : " + hologramFilePath);


            LoadHologram(hologramFilePath);
        }
    }

    private void ClearChilds()
    {
        foreach (Transform item in holograms)
        {
            Destroy(item.gameObject);
        }
    }

    public string GetHologramPath()
    {
        Debug.Log("Hologram File Path : " + hologramFilePath);
        return hologramFilePath;
    }
    
    public void LoadHologram(string Filepath, float scale = 1)
    {
        importScale = scale;
        setupUI.UpdateHoloScale(scale);
        ClearChilds(); // Remove all previously loaded objects
        
        hologramFilePath = Filepath;
        StartCoroutine(LoadBundleFromFilePath(hologramFilePath, scale));
        Debug.Log("After load Hologram path : " + hologramFilePath);
    }

    IEnumerator LoadBundleFromFilePath(string filePath, float scale = 1)
    {

        AssetBundle.UnloadAllAssetBundles(true); // Clear all asset bundles to avoid duplication errors



        var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
        Debug.Log("load bundle filepath : " + filePath);
        yield return assetBundleCreateRequest;


        AssetBundle bundle = assetBundleCreateRequest.assetBundle;

        // Get the first object in the asset bundle
        string rootAssetPath = bundle.GetAllAssetNames()[0];

        // Load into scene
        GameObject obj = Instantiate(bundle.LoadAsset(rootAssetPath) as GameObject, holograms);


        if (true)
        {
            moCapManager.CheckForMoCap(obj);
        }

        lightManager.UpdateLightWhileScaling(scale, 1, holograms);
    }
    
}
