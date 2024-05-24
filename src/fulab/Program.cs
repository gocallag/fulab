﻿using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Text;
namespace fulab;

class Program
{
    public static async Task<int> Main(params string[] args)
    {
        var globalStorage = System.Environment.GetEnvironmentVariable("FULABLOCATION", EnvironmentVariableTarget.Process);
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
                    }
                    else
                    {
                        Console.WriteLine("global storage as not been set, please review usage of \"set global\"");
                    }
                },
                newlabdefnnameOption, newlabdefnserviceOption);

        newsubCommand.Add(newlabdefnCommand);



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

        catch (Exception ex)
        {
            return false;
        }
    }
}
