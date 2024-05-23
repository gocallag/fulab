﻿using System;
using System.CommandLine;
using System.CommandLine.Invocation;
// using System.IO;
namespace fulab;

class Program
{
    public static async Task<int> Main(params string[] args)
    {
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
                    Console.WriteLine($"name={newlabdefnnameOptionValue}, service={newlabdefnserviceOptionValue}");
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
                    Console.WriteLine($"FULABLOCATION={globalstorageOptionValue}");
                },
                setglobalstoragelocationOption);

        // ---


        // rootCommand.AddOption(fileOption);

        // rootCommand.SetHandler((file) =>
        //     {
        //         ReadFile(file!);
        //     },
        //     fileOption);

        return await rootCommand.InvokeAsync(args);
    }

    static void ReadFile(FileInfo file)
    {
        File.ReadLines(file.FullName).ToList()
            .ForEach(line => Console.WriteLine(line));
    }
}