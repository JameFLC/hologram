
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveManager
{
    private static string saveExtention = ".holosave";
    private static string cameraSavePath = "/camera";
    private static string hologramSavePath = "/hologram";


    public static void SaveCameraData(float screenHeight, Vector3 cameraPosition)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + cameraSavePath + saveExtention;
        FileStream stream = new FileStream(path,FileMode.Create);
        CameraData camdata = new CameraData(screenHeight, cameraPosition);


        formatter.Serialize(stream, camdata);

        stream.Close();
    }

    public static CameraData LoadCameraData()
    {
        string path = Application.persistentDataPath + cameraSavePath + saveExtention;


        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CameraData camData = formatter.Deserialize(stream) as CameraData;
            stream.Close();

            return camData;
        }
        Debug.Log("No Camera save file available");
        return null;
    }

    public static bool isCameraSaved()
    {
        string path = Application.persistentDataPath + cameraSavePath + saveExtention;
        Debug.Log(path);
        return File.Exists(path);
    }


    public static void SaveHologram(string filePath, Vector3 offset,float HoloScale,float YRotation, int saveID)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + hologramSavePath + saveID + saveExtention;

        FileStream stream = new FileStream(path, FileMode.Create);
        HoloData holoData = new HoloData(filePath, offset, HoloScale, YRotation);


        formatter.Serialize(stream, holoData);

        stream.Close();
    }
    public static HoloData LoadHologramData(int saveID)
    {

        string path = Application.persistentDataPath + hologramSavePath + saveID + saveExtention;


        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            HoloData holoData = formatter.Deserialize(stream) as HoloData;
            stream.Close();

            return holoData;
        }
        Debug.Log("No Camera save file available");
        return null;
    }
    public static bool isHologramSaved(int saveID)
    {
        string path = Application.persistentDataPath + hologramSavePath + saveID + saveExtention;
        return File.Exists(path);
    }
}
