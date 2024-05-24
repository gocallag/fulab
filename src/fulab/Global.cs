using System.Text.Json;
namespace fulab;
class Global
{
    public string? GlobalStoragePath { get; set; }
    public Global()
    {

    }

    public Global(string gs)
    {
        GlobalStoragePath = "";
        var options = new JsonSerializerOptions { IncludeFields = true };
        if (gs != null && gs.Length != 0)
        {
            GlobalStoragePath = gs;

            using FileStream json = File.OpenRead(gs + "/.global");


            Global? g = JsonSerializer.Deserialize<Global>(json, options);
            GlobalStoragePath = g?.GlobalStoragePath;
        }

    }
    public bool SaveObject()
    {
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText(GlobalStoragePath + "/.global", jsonString);
        return true;
    }
}