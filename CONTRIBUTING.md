# Developing KuFlow.UiPathLibrary

This doc is intended for contributors to `kuflow-uipath-activities` repository. 



## Development Environment

Main tools:

- Windows 11
- Visual Studio 2022 (https://visualstudio.microsoft.com/es/vs/)
- Target Framework: net6.0-windows

The Visual Studio IDE is used for the development of this library, since UiPath provides a plugin to create a skeleton of a project of activities. This skeleton is per activity and once generated, it can only be modified via code and not through the UI of the plugin.

Some resources:

- **Intro Level:** Developer Guide: Using The Activity Creator
  - https://docs.uipath.com/developer/other/latest/developer-guide/using-activity-creator
- **Intro Level:** Custom Activities in UiPath (Activities Builder)
  - https://www.youtube.com/watch?v=34IKnoRRdgI
- **Intro Level:** UiPath Workflow Analyzer & Activity Creator: Step-by-step Tutorial
  - From 45:50m to see Activity Creator
  - https://www.youtube.com/watch?v=H2jSYbhe2tE
- **Advanced level:** Know about UiPath Forms and Activity Creator 
  - https://www.techgig.com/webinar/Know-about-UiPath-Forms-and-Activity-Creator-advanced-level-1749




## Develop

### Structure

There are 5 modules in the solution. Three are located in the path `KuFlow.UiPathLibrary` and two in the path `Shared`. In the visual studio, in the *Solution Explorer* we can observe these 5 modules.

The main one, from which we export the package that we will later import into UiPath Studio to use in robots is: `KuFlow.UiPathLibrary.Activities.Design`.

Modules:

- **KuFlow.UiPathLibrary/KuFlow.UiPathLibrary**

  - Core library, with utilities, models, services etc.

- **KuFlow.UiPathLibrary/KuFlow.UiPathLibrary.Activities**

  - Library that implements the activities

- **KuFlow.UiPathLibrary/KuFlow.UiPathLibrary.Activities.Design**

  - Library with the UI of the activities

- **Shared/UiPath.Shared.Activities**

  - Shared code of UiPath activities

- **Shared/UiPath.Shared.Activities.Design**

  - Shared UI of UiPath activities

    

### Build

To build a package:

> Important. In order to facilitate the use of dependencies in UiPath Studio, it is recommended to add the KuFlow Rest api client Nuget package in the KuFlow.UiPathLibrary.Activities.Design module. This avoids having to install this package separately in UiPath Studio.

1. Build or Rebuild the solution
2. Execute a pack on the `KuFlow.UiPathLibrary.Activities.Design` module