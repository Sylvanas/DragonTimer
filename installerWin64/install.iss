[Setup]
AppName=JungleTimer
AppVerName=JungleTimer alpha
AppPublisher=Frunza
Compression=lzma2/ultra
SolidCompression=true
AllowRootDirectory=true
DefaultDirName={pf32}\JungleTimer
DefaultGroupName=JungleTimer
EnableDirDoesntExistWarning=false
RestartIfNeededByRun=false
UninstallLogMode=new

WizardImageFile=..\Resources\InstallerImage.bmp
WizardSmallImageFile=..\Resources\SmallImage.bmp

[Languages]
Name: en; MessagesFile: compiler:Default.isl

[Files]
Source: ..\DragonTimer\bin\Debug\DragonTimer.exe; DestDir: {app}; DestName: JungleTimer.exe; Flags: ignoreversion
Source: ..\Resources\Icon.ico; DestDir: {app}; DestName: JungleTimer.ico; Flags: ignoreversion
Source: ..\License.txt; DestDir: {app}; Flags: ignoreversion
Source: ..\DragonTimer\ReadMe.pdf; DestDir: {app}; Flags: ignoreversion

[Tasks]
; NOTE: The following entry contains English phrases ("Create a desktop icon" and "Additional icons"). You are free to translate them into another language if required.
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Desktop Icons"

[Icons]
Name: {group}\JungleTimer; Filename: {app}\JungleTimer.exe; WorkingDir: {app};
Name: "{userdesktop}\JungleTimer"; Filename: "{app}\JungleTimer.exe"; IconFilename: {app}\JungleTimer.ico; WorkingDir: {app}; Tasks: desktopicon
Name: {group}\Uninstall; Filename: {uninstallexe}; WorkingDir: {app};
