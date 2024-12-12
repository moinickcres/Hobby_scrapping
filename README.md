# Hobby_scrapping

# Web Scraper Project

## Detailed Description
The Web Scraper Project automates the extraction of product data (images, prices, availability, and links) from multiple e-commerce platforms using .NET Core and Selenium. The results are displayed on a responsive React frontend and stored in MongoDB for further use.

---

## Key Technologies
- **.NET Core**: Backend for scraping orchestration and RESTful API services.
- **Selenium WebDriver**: Handles JavaScript-heavy content rendering.
- **React**: Provides an interactive and modern UI.
- **MongoDB**: Stores heterogeneous product data structures.
- **Docker**: Ensures consistent deployments across environments.
- **CI/CD**: Automates testing and deployment workflows.

---

## Architecture

### 1. Frontend:
- Utilizes **Axios** for HTTP requests.
- Responsive design with **CSS Flexbox**.
- State management via **React hooks**.

### 2. Backend:
- Multi-layered architecture:
  - **Presentation Layer**: API endpoints for frontend communication.
  - **Logic Layer**: Implements platform-specific scraping logic (e.g., `ScrappingLogic`).
  - **Data Layer**: Interacts with MongoDB.

### 3. Concurrency:
- Scraping tasks are run in parallel using **Task.WhenAll**.
- Rate limits are managed using **SemaphoreSlim**.

### 4. Testing:
- **Unit Tests**: Verifies scraping logic.
- **Integration Tests**: Validates REST API functionality.

---

## Challenges and Solutions

### 1. Rate Limits:
- Introduced random delays to prevent detection.

### 2. Dynamic Content:
- Used Selenium to handle JavaScript-rendered elements.

### 3. Flexible Storage:
- Leveraged MongoDB’s schema-less design for storing diverse product data.

---

## Results
- **Efficiency**: Implemented scalable concurrent scraping.
- **Automation**: CI/CD pipelines ensure robust testing and deployments.
- **Scalability**: Ready to expand to additional platforms.

---
