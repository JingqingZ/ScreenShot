# ScreenShot
Screen shot in C#

# Usage

```
ScreenShot.exe seconds weeks destDir
```
seconds: int, capture screen each X seconds <br/>
weeks: int, delete images older than X weeks <br/>
destDir: string, save images to this folder <br/>

# Notes

1. All images will be named by yyyy-MM-dd-HH-mm-ss.png. <br/>
2. A log.txt will be created in destination directory, and it will record each sreenshot action and exception. <br/>
3. Images older than X weeks will be deleted automatically <br/>