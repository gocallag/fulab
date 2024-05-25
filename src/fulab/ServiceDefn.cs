using System.Text.Json;
namespace fulab;
class ServiceDefn
{
    public string? Name { get; set; }
    public string? Hypervisor { get; set; }
    public string? Host { get; set; }
    public int? Port { get; set; }
    public bool? Insecure { get; set; }
    public string? GlobalStoragePath { get; set; }
    
    public ServiceDefn(string gs)
    {
        GlobalStoragePath = gs;
    }

    public ServiceDefn(string gs, string name, string hypervisor, string host, int port, bool insecure)
    {
        ServiceDefn? g = new ServiceDefn(gs);
        var options = new JsonSerializerOptions { IncludeFields = true };

        try
        {
            using FileStream json = File.OpenRead($"{gs}/servicedefn-{name}");
            g = JsonSerializer.Deserialize<ServiceDefn>(json, options);
        }
        catch (Exception)
        {
            g = new ServiceDefn(gs);
        }

        GlobalStoragePath = gs;
        Name ??= name;
        Hypervisor ??= hypervisor;
        Host ??= host;
        Port ??= port;
        Insecure ??= insecure;

    }
    public bool SaveObject()
    {
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText($"{GlobalStoragePath}/servicedefn-{Name}", jsonString);
        return true;
    }
}