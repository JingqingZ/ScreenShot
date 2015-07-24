# ScreenShot
Screen shot in C#

# Usage

```
ScreenShot.exe seconds limit destinationDirectory
```
seconds: Capture screen every X seconds <br/>
limit: Stop when X images have been captured. If limit == -1, the program will never stop until Ctrl-C interrupts.<br/>
destinationDirectory: all images captured will be saved in this directory, which will be created and cleaned automatically <br/>

# Notes

1. All images will be named by yyyy-MM-dd-HH-mm-ss.png. <br/>
2. A log.txt will be created in destination directory, and it will record each sreenshot action and exception. <br/>