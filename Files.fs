(*
    Copyright (C) 2023  Fredrik Holmqvist

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.`
*)

module Files

open System.IO
open System.Diagnostics

let findEntriesFolder () =
    let name =
        ProcessStartInfo("bash", "-c \"echo $USER\"")
        |> fun info ->
            info.RedirectStandardOutput <- true
            info.UseShellExecute <- false
            info.CreateNoWindow <- true
            Process.Start(info).StandardOutput.ReadToEnd().Trim()

    $"/home/{name}/mplanner"

let latestEntry path =
    Directory.GetFiles(path)
    |> Array.filter (fun name -> name.Contains(".md"))
    |> Array.sort
    |> Array.tryLast
    |> Option.map File.ReadAllLines
    |> Option.map Array.toList

let entriesFolder = findEntriesFolder ()
let now = System.DateTime.UtcNow.ToString "yyyy-MM-dd"
let latestFilePath = $"{entriesFolder}/{now}.md"
let header = $"# {now}"

let writeIfNotExists path (lines: string list) =
    match File.Exists(path) with
    | true -> ()
    | false -> File.WriteAllLines(path, lines)

let writeTodoFile path (todos: string list list) =
    [ [ header ] ] @ todos
    |> List.map (fun list -> list @ [ "" ])
    |> List.collect id
    |> writeIfNotExists path
