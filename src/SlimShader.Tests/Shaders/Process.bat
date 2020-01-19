@ECHO OFF
rem "C:\Program Files (x86)\Windows Kits\10\bin\10.0.18362.0\x86\fxc" Internal\BasicHLSL.hlsl /Fo test.o /Fc test.asm /T ps_5_0 /E PSMain /nologo 
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.18362.0\x86\fxc" Internal\BasicHLSL.hlsl /P test.hlsl /nologo 