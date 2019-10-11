for /r /d %%a in (*) do if /i "%%~nxa"=="bin" rd /s /q "%%a"
for /r /d %%a in (*) do if /i "%%~nxa"=="obj" rd /s /q "%%a"
rd /s /q ".vs"
