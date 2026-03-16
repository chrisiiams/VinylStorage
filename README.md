# VinylStorage

A desktop app to manage your vinyl record collection — built with WPF and C# on .NET 10.

---

## Background

This project grew out of a phonebook app I built during school. I liked the structure and had the idea for another App. I have a ton of Vinyls and i thought it nifty to have my collection stored in a Database. I swapped contacts for vinyl records, expanded the data model, and added a few things I wanted to learn: asynchronous programming and external API integration.

---

## Features

- **Add, edit, delete** records in your collection
- **Search** by artist or album title in real time
- **Album cover art** fetched automatically via the [MusicBrainz](https://musicbrainz.org/) + [Cover Art Archive](https://coverartarchive.org/) APIs when you open a record
- Cover URLs are **cached in the local database** so they don't re-fetch every time
- Data is persisted locally in a **SQLite** database

---

## Tech Stack

| | |
|---|---|
| Language | C# |
| UI Framework | WPF (.NET 10) |
| Database | SQLite via `Microsoft.Data.Sqlite` |
| Cover Art | MusicBrainz API + Cover Art Archive |
| Architecture | OOP — separate model, data access, and UI layers |

---

## What I Learned / Applied

- **Asynchronous programming** — cover art is fetched with `async/await` so the UI stays responsive while waiting on the API
- **OOP** — clean separation between the `Vinyl` model, the `Data` access class, and the UI windows
- **REST API consumption** — querying MusicBrainz, parsing JSON responses, and handling cases where no result is found
- **SQLite integration** — parameterized queries, schema creation on startup, caching remote data locally

---

## Planned

- I was thinking maybe a GUI inspired by Y2K-Asthetic
- Album covers visible in the main list view
- Album descriptions pulled from Wikipedia in the edit window
- Published Version for you to run from a .exe

---

## Build from Source

Requirements: Visual Studio 2022+, .NET 10 SDK

```bash
git clone https://github.com/yourusername/VinylStorage.git
```

Open `VinylStorage.sln` in Visual Studio, restore NuGet packages, and hit **F5**.
