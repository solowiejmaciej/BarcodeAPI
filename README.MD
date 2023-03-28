# API Documentation

This API allows users to add, login and retrieve product information.

## Endpoints

###### POST /api/account/add

###### POST /api/account/login

###### GET /api/barcode/{ean}

## Add API user

##### POST /api/account/add

###### Adds a new API user to the database. The request must include a JSON object in the body with the following fields:  
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "password": "string"
}
```

## User Login

##### POST /api/account/login

###### Allows an API user to obtain a JWT token for API usage. The request must include a JSON object in the body with the following fields:

```json
{
  "email": "string",
  "password": "string"
}
```
## Response
###### If the request is successful, the API returns a JSON object with a JWT token:

```json
{
  "token": "string",
  "statusCode": 200,
  "expiresAt": DateTime,
  "issuedAt": DateTime
}
```

## Retrieve Product Information

##### GET /api/barcode/{ean}

###### Retrieves product information from the database and OpenFoodApi. The endpoint takes an EAN code as a parameter in the URL.  

###### If the product is found in the database or OpenFoodApi, the API returns a JSON object with the product information:

```json
{
    "name": "string",
    "price": decimal,
    "brandName": "string",
    "ean": "string"
}
```
