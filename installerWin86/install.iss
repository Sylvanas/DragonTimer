[Setup]
AppName=DragonTimer
AppVerName=DragonTimer alpha
AppPublisher=Frunza
Compression=lzma2/ultra
SolidCompression=true
AllowRootDirectory=true
DefaultDirName={pf32}\DragonTimer
DefaultGroupName=DragonTimer
EnableDirDoesntExistWarning=false
RestartIfNeededByRun=false
UninstallLogMode=new

WizardImageFile=..\Resources\InstallerImage.bmp
WizardSmallImageFile=..\Resources\SmallImage.bmp

[Languages]
Name: en; MessagesFile: compiler:Default.isl

[Files]
Source: ..\DragonTimer\bin\Debug\DragonTimer.exe; DestDir: {app}; Flags: ignoreversion

[Tasks]
; NOTE: The following entry contains English phrases ("Create a desktop icon" and "Additional icons"). You are free to translate them into another language if required.
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Desktop Icons"

[Icons]
Name: {group}\DragonTimer; Filename: {app}\DragonTimer.exe; WorkingDir: {app};
Name: "{userdesktop}\DragonTimer"; Filename: "{app}\DragonTimer.exe"; IconFilename: {app}\dragonTimer.ico; WorkingDir: {app}; Tasks: desktopicon
Name: {group}\Uninstall; Filename: {uninstallexe}; WorkingDir: {app};
