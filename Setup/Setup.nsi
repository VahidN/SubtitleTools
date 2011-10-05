!include "FileAssociation.nsh"

; The name of the installer
Name "SubtitleTools 1.8"

; The file to write

OutFile "SubtitleTools_1.8_setup.exe"
Caption "SubtitleTools 1.8 Installer"
XPStyle on
SetDatablockOptimize on
CRCCheck force

SetCompressor /SOLID /FINAL lzma

; The default installation directory
InstallDirRegKey HKLM "Software\SubtitleTools" "folder"
Icon "${NSISDIR}\Contrib\Graphics\Icons\orange-install.ico"

;--------------------------------

; Pages
Page directory
Page instfiles

ShowInstDetails show
AutoCloseWindow true
;--------------------------------

; The stuff to install
Section "" ;No components page, name is not important

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR      
  SetOverwrite try ; NOT AN INSTRUCTION, NOT COUNTED IN SKIPPINGS  
  
  File ..\SubtitleTools\bin\Release\Ionic.Zlib.dll
  File ..\SubtitleTools\bin\Release\SubtitleTools.vshost.exe.manifest
  File ..\SubtitleTools\bin\Release\CookComputing.XmlRpcV2.dll
  File ..\SubtitleTools\bin\Release\SubtitleTools.exe.config
  File ..\SubtitleTools\bin\Release\SubtitleTools.vshost.exe.config
  File ..\SubtitleTools\bin\Release\SubtitleTools.Common.dll
  File ..\SubtitleTools\bin\Release\SubtitleTools.Infrastructure.dll
  File ..\SubtitleTools\bin\Release\SubtitleTools.exe
  File ..\SubtitleTools\bin\Release\SubtitleTools.Shell.dll
  File ..\SubtitleTools\bin\Release\SubtitleTools.vshost.exe
  File ..\SubtitleTools\bin\Release\EncodingTools.dll
  File ..\SubtitleTools\bin\Release\PersianProofWriter.Lib.dll
  File "C:\Program Files\Microsoft SDKs\Expression\Blend\.NETFramework\v4.0\Libraries\Microsoft.Expression.Interactions.dll"
  File "C:\Program Files\Microsoft SDKs\Expression\Blend\.NETFramework\v4.0\Libraries\System.Windows.Interactivity.dll"  
  
  WriteRegStr HKLM "Software\SubtitleTools" "folder" $INSTDIR
  
  ;create desktop shortcut
  CreateShortCut "$DESKTOP\SubtitleTools.lnk" "$INSTDIR\SubtitleTools.exe" ""
  
  ;Set File Association
  ${registerExtension} "$INSTDIR\SubtitleTools.exe" ".srt" "Subtitle_File"
  
  MessageBox MB_OK "You have successfully installed SubtitleTools. Please use the desktop icon to start the program."
  
SectionEnd ; end the section


