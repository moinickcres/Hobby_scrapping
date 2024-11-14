import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [searchTerm, setSearchTerm] = useState("");
  const [products, setProducts] = useState([]);

  /*let config = {
    headers: {
      'Access-Control-Allow-Origin': '*'
    }
  }*/

  const handleSearch = async () => {
    try {
      const response = await axios.post("https://localhost:7246/api/scrape", { searchTerm });
      const sortedProducts = response.data.sort((a, b) => a.price - b.price); // Sort by price (ascending)
      setProducts(sortedProducts); // Assume backend returns an array of product data
      console.log("Fetched product data:", sortedProducts);
    } catch (error) {
      console.error("Error fetching product data:", error);
    }
  };

  return (
    <div className="app-container">
      <h1>Product Scraper</h1>
      <div className="search-bar">
        <input
          type="text"
          placeholder="Search for a product..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
        <button onClick={handleSearch}>Search</button>
      </div>

      <h2>Search Results</h2>
      <div className="product-list">
        {products.map((product, index) => (
          <div key={index} className="product-card">
            <a href={product.link} target="_blank" rel="noopener noreferrer">
              <img src={product.imageUrl} alt={product.name} className="product-image" />
            </a>
            <div className="product-details">
              <a href={product.link} target="_blank" rel="noopener noreferrer" className="product-name">
                {product.name}
              </a>
              <p className="product-price">Price: {product.price.toFixed(2)}€</p>
              <p className={`product-stock ${product.inStock ? 'in-stock' : 'out-of-stock'}`}>
                {product.inStock ? "In Stock" : "Out of Stock"}
              </p>
              <p className="product-page">Page: {product.pageName}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default App;
