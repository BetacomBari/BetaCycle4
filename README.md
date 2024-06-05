# Program Class

**Purpose:** Serves as the entry point for the application.

**Functionality:**
- Configures dependency injection container, middleware, and application settings.
- Starts the web application.

**Services:**
- Registers various services like `DbTracer`, `JwtSettings`, `EmailService`, and database context (`AdventureWorksLt2019Context`).

**Additional Notes:**
- Implements basic and JWT authentication.
- Uses Swagger for API documentation.

# HttpRequestService Class

**Purpose:** Handles HTTP requests to interact with a backend server.

**Functionality:**
- Implements methods for user authentication, customer management, product management, and shopping cart operations.

**Methods:**
- Includes methods like `loginPostJwt`, `register`, `getCustomer`, `getProduct`, etc.
- Each method corresponds to an HTTP endpoint on the backend server.

**Dependencies:**
- Relies on `HttpClient` for making HTTP requests and `AuthService` for authentication.


# DbUtilityForCart Class

**Purpose:** Manages database operations related to shopping cart functionality.

**Functionality:**
- Implements CRUD operations for shopping cart items.
- Handles database connections and executes SQL commands.

**Methods:**
- Includes methods like `GetCartByCustomerId`, `AddProductToCart`, `BuyProductInYourCart`, etc.

**Dependencies:**
- Relies on `SqlConnection` and `SqlCommand` for database operations.

# Customer and CustomerNew Classes

**Purpose:** Represent customer data within the application.

**Functionality:**
- Store information about customers such as name, email, and contact details.
- `CustomerNew` likely extends `Customer` with additional properties.

**Properties:**
- Include attributes like `CustomerId`, `Name`, `EmailAddress`, etc.

**Associations:**
- `Customer` class likely has associations with other entities like `CustomerAddress` and `SalesOrderHeader`.

# CredentialDB Class

**Purpose:** Represents user credentials stored in the database.

**Functionality:**
- Stores encrypted email addresses, hashed passwords, and password reset tokens.

**Properties:**
- Includes attributes like `EmailAddressEncrypt`, `PasswordHash`, `ResetPasswordToken`, etc.

# LogError Class

**Purpose:** Models errors that occur within the application.

**Functionality:**
- Captures details about errors such as error message, code, and location.

**Properties:**
- Includes attributes like `Id`, `ErrorMessage`, `ErrorCode`, `ErrorDate`, etc.


# Controllers

## CustomersController

**Purpose:** Manages HTTP requests related to customers in the web API.

**Functionality:**
- Supports CRUD operations on customer data.
- Implements endpoints for retrieving, creating, updating, and deleting customers.

**Methods:**
- Includes methods like `GetCustomers`, `GetCustomer`, `PostCustomer`, `PutCustomer`, `DeleteCustomer`, etc.

**Security:**
- Requires authorization for most methods.
- Handles error handling for concurrency exceptions during updates.

## CustomersNewController

**Purpose:** Handles HTTP requests related to customer data in the web API.

**Functionality:**
- Provides endpoints for CRUD operations on a newer version of customer data.
- Introduces functionality for sending password reset emails and resetting passwords.

**Methods:**
- Includes methods like `GetCustomerNews`, `GetCustomerNew`, `PostCustomerNew`, `PutCustomerNew`, `send-reset-email`, `reset-password`, etc.

**Security:**
- Requires authorization for most methods.
- Implements password reset functionality securely.

## LoginController

**Purpose:** Manages login functionality for the web API.

**Functionality:**
- Validates user credentials and generates JWT tokens for successful logins.
- Supports user accounts stored in multiple databases.

**Methods:**
- Includes methods like `Login`, `GenerateJwtToken`, `CheckEmailDbAWLT2019`, `CheckEmailDbCustomerCredentials`, etc.

**Security:**
- Implements basic and JWT authentication.
- Handles error logging and response generation for login requests.


# `DbUtility` Class Summary

The `DbUtility` class is a utility for managing database operations using SQL Server. It handles the connection and SQL commands required to interact with the database. The main functionalities of this class include:

## Credential Management
- **Update Credential ID**: Updates the credential ID.
- **Check Email Existence**: Checks if an email exists in the customer credential database.
- **Retrieve Credentials**: Retrieves credentials using an encrypted email.
- **Insert and Update Credentials**: Inserts and updates credential information in the database.
- **Retrieve Password Hash and Salt**: Retrieves the password hash and salt for an encrypted email.
- **Manage Password Reset Tokens**: Handles password reset tokens.

# `DbUtilityForCart` Class Summary

The `DbUtilityForCart` class is a utility for managing shopping cart operations using SQL Server. It handles the connection and SQL commands required to interact with the database. The main functionalities of this class include:

## Constructor
- **Initialize Connection**: Initializes the database connection using a connection string and checks the connection status.

## Shopping Cart Management
- **Get Cart by Customer ID**: Retrieves the products in the shopping cart for a given customer.
- **Add Product to Cart**: Adds a product to the customer's shopping cart.
- **Purchase Products in Cart**: Marks the products in the customer's cart as purchased.
- **Delete Product from Cart**: Deletes a product from the customer's shopping cart.

## Product Information
- **Get Product Information**: Retrieves detailed information about a product using its ID.

## Customer Information
- **Select Customer ID**: Retrieves the customer ID using the encrypted email address.
- **Select Address ID**: Retrieves the address ID associated with the customer ID.

## Database Connection Management
- **Check Database Open/Close**: Checks and manages the database connection state.





# AI CHATBOT
# How Does It Work?

## Define Intents
We define our intents in `intents.json`, with tags and patterns. This helps in training the model.

## NLTK
We utilize the NLTK library to work with words (`nltk_utils.py`). Specifically, we use NLTK for tokenization and stemming the text.

## Create Our Neural Network
We employ a feed-forward neural network. The input layer is fully-connected with the number of patterns as its dimension, followed by 2 hidden layers, and an output layer with the number of classes as its dimension. We apply softmax to obtain the response, which is given if it has at least a 75% confidence according to the training part.

## Training of the Model
To train the model (`train.py`), we import the functions written in `nltk_utils.py`. PyTorch is used for training.

## Chatbot
The logic of the chatbot is found in `app.js`. The chatbot, named Yoghi, initially presents its capabilities with a welcome message. It then responds to typical questions one might ask a VPN service. If the message exceeds 100 characters or the question is outside its training scope, it replies with an error message.

## Connect to the Website
We use Flask (`app.py`) to create our API. With `@app.get("/")`, we return our webpage's template, and with `@app.post("/predict")`, we predict the response to the question asked, based on the logic and training part.




*RECOMMENDER SYSTEM

**Data Pre-processing
We are going to use a technique called colaborative filtering to generate recommendations for users. This technique is based on the premise that similar people like similar things.

First we need to create the dataset, so we have generate 1000 rows with CustomerId, ProductId and a generic rate value from 0 to 5 `recommenderSystem.py`

Then we transform our data into a user-item matrix, also known as a "utility" matrix. In this matrix, rows represent users and columns represent item bought. The beauty of collaborative filtering is that it doesn't require any information about the users or the item user to generate recommendations.

The `create_X()` function outputs a sparse matrix $X$ with four mapper dictionaries:

- **customer_mapper**: maps user id to user index
- **product_mapper**: maps movie id to movie index
- **customer_inv_mapper**: maps user index to user id
- **product_inv_mapper**: maps movie index to movie id

We need these dictionaries because they map which row/column of the utility matrix corresponds to which user/movie id.

### Evaluating sparsity

Here, we calculate sparsity by dividing the number of stored elements by total number of elements. The number of stored (non-empty) elements in our matrix ([nnz](https://docs.scipy.org/doc/scipy/reference/generated/scipy.sparse.csr_matrix.nnz.html)) is equivalent to the number of ratings in our dataset.

###Item-item Recommendations with k-Nearest Neighbors
We are going to find the $k$ product that have the most similar user engagement vectors for product $i$.

![Alt text](./sparse_matrix.png/to/img.jpg?raw=true "Sparse Matrix")

