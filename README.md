# ProjectClone 
[![Release](https://img.shields.io/github/v/release/hankangwen/ProjectClone)](https://github.com/hankangwen/ProjectClone/releases) 
[![Documentation](https://img.shields.io/badge/documentation-brightgreen.svg)](https://github.com/hankangwen/ProjectClone/wiki) 
[![License](https://img.shields.io/badge/license-MIT-green)](https://github.com/hankangwen/ProjectClone/blob/main/LICENSE) 
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blue.svg)](https://github.com/hankangwen/ProjectClone/pulls) 
[![Chats](https://img.shields.io/discord/710688100996743200)](https://discord.gg/TmQk2qG)  

ProjectClone is a Unity editor extension that allows users to test multiplayer gameplay without building the project by having another Unity editor window opened and mirror the changes from the original project.  

为Unity多开工程提供一种解决方案。  
使用微软的mklink创建软联接。

<br>

![ShortGif](https://raw.githubusercontent.com/VeriorPies/ParrelSync/master/Images/Showcase%201.gif)
<p align="center">
<b>Test project changes on clients and server within seconds - both in editor
</b>
<br>
</p>

## Features
1. Test multiplayer gameplay without building the project
2. GUI tools for managing all project clones
3. Protected assets from being modified by other clone instances
4. Handy APIs to speed up testing workflows
## Installation

### UPM Package
ProjectClone can also be installed via UPM package.  
After Unity 2019.3.4f1, Unity 2020.1a21, which support path query parameter of git package. You can install ParrelSync by adding the following to Package Manager.

```
https://github.com/hankangwen/ProjectClone.git?path=/Assets/ProjectClone
```  

  
![UPM_Image](https://github.com/VeriorPies/ParrelSync/raw/master/Images/UPM_1.png?raw=true) ![UPM_Image2](https://github.com/VeriorPies/ParrelSync/raw/master/Images/UPM_2.png?raw=true)
  
or by adding 

```
"com.hankangwen.projectclone": "https://github.com/hankangwen/ProjectClone.git?path=/Assets/ProjectClone"
``` 

to the `Packages/manifest.json` file 


## Supported Platform
Currently, ParrelSync supports Windows, macOS and Linux editors.  

![image](https://user-images.githubusercontent.com/46420877/203921117-c7929b10-7167-41ea-bd0c-f30d57d668f2.png)

![image](https://user-images.githubusercontent.com/46420877/203921118-b633792e-72fe-4b30-b640-9f14a2bdb84f.png)

![image](https://user-images.githubusercontent.com/46420877/203921165-f1289e5b-bb4a-4b27-9c71-a8fedd7a3946.png)

