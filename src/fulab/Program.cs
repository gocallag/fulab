using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Text;
using System.Text.Json;
namespace fulab;

class Program
{
    public static async Task<int> Main(params string[] args)
    {
        Global? global;

        string? globalStorage = System.Environment.GetEnvironmentVariable("FULABLOCATION", EnvironmentVariableTarget.Process);

        global = new Global(globalStorage);

        var fileOption = new Option<FileInfo?>(
            name: "--file",
            description: "The file to read and display on the console.");

        var rootCommand = new RootCommand("fulab - lab builder");

        // new command

        var newsubCommand = new Command("new", "create a new object");
        rootCommand.Add(newsubCommand);

        // new lab-defn

        var newlabdefnCommand = new Command("lab-defn", "create a new lab definition");

        var newlabdefnnameOption = new Option<string>("--name", "the name of the lab definition") { IsRequired = true };
        newlabdefnnameOption.AddAlias("-n");
        newlabdefnCommand.Add(newlabdefnnameOption);

        var newlabdefnserviceOption = new Option<string>("--service", "the service that provides the lab") { IsRequired = true };
        newlabdefnserviceOption.AddAlias("-s");
        newlabdefnCommand.Add(newlabdefnserviceOption);
        newlabdefnCommand.SetHandler((newlabdefnnameOptionValue, newlabdefnserviceOptionValue) =>
                {
                    if (globalStorage != null && globalStorage.Length != 0)
                    {

                        Console.WriteLine($"location={globalStorage},name={newlabdefnnameOptionValue}, service={newlabdefnserviceOptionValue}");
                        var labdefn = new LabDefn(globalStorage, newlabdefnnameOptionValue, newlabdefnserviceOptionValue);
                        labdefn.SaveObject();
                    }
                    else
                    {
                        Console.WriteLine("global storage as not been set, please review usage of \"set global\"");
                    }
                },
                newlabdefnnameOption, newlabdefnserviceOption);

        newsubCommand.Add(newlabdefnCommand);

        // new service-defn

        var newservicedefnCommand = new Command("service-defn", "create a new service definition");

        var newservicedefnnameOption = new Option<string>("--name", "the name of the service definition") { IsRequired = true };
        newservicedefnnameOption.AddAlias("-n");
        newservicedefnCommand.Add(newservicedefnnameOption);

        var newservicedefnhypervisorOption = new Option<string>("--hypervisor", "the hypervisor that provides the service") { IsRequired = true };
        newservicedefnhypervisorOption.AddAlias("-h");
        newservicedefnCommand.Add(newservicedefnhypervisorOption);
        newservicedefnCommand.SetHandler((newservicedefnnameOptionValue, newservicedefnhypervisorOptionValue) =>
                {
                    if (globalStorage != null && globalStorage.Length != 0)
                    {
                        var servicedefn = new ServiceDefn(globalStorage, newservicedefnnameOptionValue, newservicedefnhypervisorOptionValue);
                        servicedefn.SaveObject();
                    }
                    else
                    {
                        Console.WriteLine("global storage as not been set, please review usage of \"set global\"");
                    }
                },
                newservicedefnnameOption, newservicedefnhypervisorOption);

        newsubCommand.Add(newservicedefnCommand);

        // ---

        // set command

        var setsubCommand = new Command("set", "set a new object");
        rootCommand.Add(setsubCommand);

        // set global

        var setglobalCommand = new Command("global", "set a global object");

        setsubCommand.Add(setglobalCommand);

        // set global storage
        var setglobalstorageCommand = new Command("storage", "set global storage");
        var setglobalstoragelocationOption = new Option<string>("--location", "where to store the lab definitions") { IsRequired = true };
        setglobalstoragelocationOption.AddAlias("-l");
        setglobalstorageCommand.Add(setglobalstoragelocationOption);

        setglobalCommand.Add(setglobalstorageCommand);
        setglobalstorageCommand.SetHandler((globalstorageOptionValue) =>
                {
                    if (testStorage(globalstorageOptionValue))
                    {
                        Console.WriteLine($"FULABLOCATION={globalstorageOptionValue}");
                        global.GlobalStoragePath = globalstorageOptionValue;
                        global.SaveObject();

                    }
                    else
                    {
                        Console.Error.WriteLine($"Unable to write to global storage location {globalstorageOptionValue}");
                        System.Environment.Exit(1);
                    }
                },
                setglobalstoragelocationOption);

        // ---

        return await rootCommand.InvokeAsync(args);
    }

    static bool testStorage(string gs)
    {
        if (gs == null || gs.Length == 0) return false;

        try
        {
            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create(gs + "/.test"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("Test FULAB write.");
                fs.Write(info, 0, info.Length);
            }
            return true;
        }

        catch (Exception)
        {
            return false;
        }
    }

}
