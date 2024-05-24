using System.Text.Json;
namespace fulab;
class LabDefn
{
    public string? Name { get; set; }
    public string? Service { get; set; }
    public string? GlobalStoragePath { get; set; }
    public LabDefn(string gs)
    {
        GlobalStoragePath = gs;
    }

    public LabDefn(string gs, string name, string service)
    {
        LabDefn? g = new LabDefn(gs);
        var options = new JsonSerializerOptions { IncludeFields = true };


        try
        {
            using FileStream json = File.OpenRead($"{gs}/labdefn-{name}");
            g = JsonSerializer.Deserialize<LabDefn>(json, options);
        }
        catch (Exception)
        {
            g = new LabDefn(gs);
        }

        GlobalStoragePath = gs;
        Name ??= name;
        Service ??= service;

    }
    public bool SaveObject()
    {
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText($"{GlobalStoragePath}/labdefn-{Name}", jsonString);
        return true;
    }
}