# **4createassignment**

## **Description**
This project represents a containerized application consisting of two Docker images:

- **Sks365.Offer.Hub.API** - the application that manages business logic and API.
- **SQL Server 2022** - the database used for data storage.

## **Requirements**
- [Docker](https://www.docker.com/get-started) (version 20.10 and above)
- [Docker Compose](https://docs.docker.com/compose/) (version 1.28 and above)

## **Instructions to Run the Application**

1. **Clone the Repository**
   ```bash
   git clone https://github.com/zevelickovic/4createassignment
   ```
2. **Navigate to the Root Folder of the Project**
   ```bash
   cd 4createassignment
3. **Build and Run Containers**
   ```bash
   docker-compose up --build -d
   ```  
4. **Access Swagger UI for API Testing**  
   Open a web browser and go to:
   ```bash
   http://localhost:8080/swagger/index.html
   ```
