# Purchases calculator

This is a Minimal API project that provides endpoints to create and retrieve purchases. The project includes endpoints to perform CRUD operations and calculations related to purchases, gross amout, net amout and vat amout.
___

## Endpoints

### GET /gb/purchase

**Summary:**
Fetches all purchases from the system.

**Response:**
- **Status 200 OK:** Returns a list of all purchases.

#### Example Response:

```json
[
  {
    "id": 1,
    "net": 100.50,
    "gross": 120.60,
    "vat": 20.10,
    "vatRate": 20
  },
  {
    "id": 2,
    "net": 200.00,
    "gross": 240.00,
    "vat": 40.00,
    "vatRate": 20
  }
]
```
___

### GET /gb/purchase/{id}
**Summary:**
Fetches a purchase by its unique ID.

**Parameters:**
- Id: The ID of the purchase to retrieve.

**Response:**
- **Status 200 OK:** Returns a list of all purchases.
- **Status 404 Not Found:** If no purchase is found with the specified ID.

#### Example Response:

```json
{
  "id": 1,
  "net": 100.50,
  "gross": 120.60,
  "vat": 20.10,
  "vatRate": 20
}
```
___

### POST /gb/purchase
**Summary:**
Creates a new purchase entry.

**Requirement**
- Only acepts one value of the three (Gross, Net or Vat) and VatRate
- VatRate can only be 10, 13 or 20

#### Request Body:

```json
{
  "net": 100.50,
  "vatRate": 20
}
```

**Response:**
- **Status 201 Created:** Returns the created purchase's details.

#### Example Response:

```json
{
  "id": 1,
  "net": 100.50,
  "gross": 120.60,
  "vat": 20.10,
  "vatRate": 20
}
```
