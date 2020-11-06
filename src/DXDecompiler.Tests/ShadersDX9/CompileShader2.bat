@ECHO OFF

"C:\Program Files (x86)\Windows Kits\10\bin\10.0.18362.0\x86\fxc" %1/%2 /Fo %1/%3.o /T %4 /nologo %5 %6
"C:\Program Files (x86)\Microsoft DirectX SDK (June 2010)\Utilities\bin\x86\fxc" /dumpbin %1/%3.o /Fc %1/%3.asm /T %4 /nologo %5 %6

IF %errorlevel% NEQ 0 EXIT /b %errorlevel%