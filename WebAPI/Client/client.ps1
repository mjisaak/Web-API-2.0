$x = [guid]::NewGuid()

Invoke-WebRequest -Uri "https://localhost/WebAPI/api/default/$x/helloFred" -Method Post

#Invoke-WebRequest -Uri 'https://localhost/WebAPI/api/default' | select StatusCode, Content
#Invoke-WebRequest -Uri 'https://localhost/WebAPI/api/default/hello, world' | select StatusCode, Content