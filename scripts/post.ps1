$uri = "http://localhost:8000/resources";



Invoke-RestMethod -Uri $uri -Method Post -Body "{  
   name : 'test'   
}"