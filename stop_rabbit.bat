ECHO OFF
for /f "delims=" %%a in ('docker ps -q -f name^=some-rabbit') do @set @ID=%%a
docker stop %@ID%
docker rm %@ID%
ECHO ON