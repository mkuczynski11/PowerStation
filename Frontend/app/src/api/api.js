const axios = require("axios").default;
const API_URL = "http://localhost:8080/api";

export async function getSensorData(params) {
  console.log(`Fetching JSON sensor data ${API_URL}`);

  let parsedParams = {};

  for (const key in params) {
    if (params[key] != false) {
      parsedParams[key] = params[key];
    }
  }

  let res = await axios
    .get(`${API_URL}/data`, {
      params: {
        ...parsedParams,
      },
    })
    .then((response) => {
      return response;
    });

  return res.data;
  // return [
  //   {
  //     "sensorID": 0,
  //     "sensorType": "core-temperature",
  //     "timestamp": 3,
  //     "value": 12
  //   },
  //   {
  //     "sensorID": 1,
  //     "sensorType": "core-temperature",
  //     "timestamp": 12,
  //     "value": 123
  //   },
  //   {
  //     "sensorID": 3,
  //     "sensorType": "core-temperature",
  //     "timestamp": 3,
  //     "value": 12
  //   },
  //   {
  //     "sensorID": 4,
  //     "sensorType": "core-temperature",
  //     "timestamp": 12,
  //     "value": 123
  //   },
  //   {
  //     "sensorID": 55,
  //     "sensorType": "core-temperature",
  //     "timestamp": 3,
  //     "value": 12
  //   },
  //   {
  //     "sensorID": 1,
  //     "sensorType": "core-temperature",
  //     "timestamp": 12,
  //     "value": 123
  //   },
  //   {
  //     "sensorID": 3,
  //     "sensorType": "power-generated",
  //     "timestamp": 14,
  //     "value": 1300
  //   }
  // ]
}

export async function getSensorDataCSV(params) {
  console.log(`Fetching CSV sensor data ${API_URL}`);

  let parsedParams = {};

  for (const key in params) {
    if (params[key] != false) {
      parsedParams[key] = params[key];
    }
  }

  let res = await axios
    .get(`${API_URL}/data/csv`, {
      params: {
        ...parsedParams,
      },
    })
    .then((response) => {
      return response;
    });

  return res.data;
}

export async function getBooks() {
  console.log(`Fetching all books from ${API_URL}`);
  let res = await axios
    .get(`${API_URL}/warehouse/books`, {
      params: {},
    })
    .then((response) => {
      return response;
    });

  return res.data;
}

export async function getBook(id) {
  console.log(`Fetching one book from ${API_URL}`);
  let res = await axios
    .get(`${API_URL}/warehouse/books/${id}`, {
      params: {},
    })
    .then((response) => {
      return response;
    });

  return res.data;
}

export async function getBooksPrices() {
  console.log(`Fetching price for books from ${API_URL}`);
  let res = await axios
    .get(`${API_URL}/sales/books`, {
      params: {},
    })
    .then((response) => {
      return response;
    });

  return res.data;
}

export async function getBooksDiscounts() {
  console.log(`Fetching discounts for books from ${API_URL}`);
  let res = await axios
    .get(`${API_URL}/marketing/books`, {
      params: {},
    })
    .then((response) => {
      return response;
    });

  return res.data;
}

export async function getShippingMethods() {
  console.log(`Fetching all shipping methods from ${API_URL}`);
  let res = await axios
    .get(`${API_URL}/shipping/methods`, {
      params: {},
    })
    .then((response) => {
      return response;
    });

  return res.data;
}

export async function createOrder(order) {
  console.log(`Creating order for book ${order.book.name} ${API_URL}`);
  let res = await axios
    .post(
      `${API_URL}/contact/orders/create`,
      {
        BookID: order.book.id,
        BookName: order.book.name,
        BookQuantity: order.quantity,
        BookPrice: order.book.unitPrice,
        BookDiscount: order.book.discount / 100,
        DeliveryMethod: order.deliveryMethod.method,
        DeliveryPrice: order.deliveryMethod.price,
      },
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function createBook(book) {
  console.log(`Creating book ${book.name} ${API_URL}`);
  let res = await axios
    .post(
      `${API_URL}/warehouse/books/create`,
      {
        name: book.name,
        price: parseFloat(book.unitPrice),
        quantity: parseInt(book.quantity),
        discount: parseInt(book.discount) / 100,
      },
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function createShipping(method) {
  console.log(`Creating shipping method ${method.name} ${API_URL}`);
  let res = await axios
    .post(
      `${API_URL}/shipping/methods`,
      {
        Method: method.name,
        Price: parseFloat(method.price),
      },
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function deleteShippingMethod(method) {
  console.log(`Deleting shipping method ${method.name} ${API_URL}`);
  let res = await axios
    .delete(
      `${API_URL}/shipping/methods`,
      {
        data: {
          Method: method.name,
        },

        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function updateBookInWarehouse(book) {
  console.log(`Updating book ${book.name} in warehouse ${API_URL}`);
  let res = await axios
    .put(
      `${API_URL}/warehouse/books`,
      {
        ID: book.id,
        name: book.name,
        quantity: parseInt(book.quantity),
      },
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function updateBookPrice(book) {
  console.log(`Updating price of book ${book.name} ${API_URL}`);
  let res = await axios
    .put(
      `${API_URL}/sales/books`,
      {
        BookID: book.id,
        BookPrice: parseFloat(book.unitPrice),
      },
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function updateBookDiscount(book) {
  console.log(`Updating discount of book ${book.name} ${API_URL}`);
  let res = await axios
    .put(
      `${API_URL}/marketing/books`,
      {
        BookID: book.id,
        BookDiscount: parseInt(book.discount) / 100,
      },
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function confirmOrder(order) {
  console.log(`Confirming order ${order.id} ${API_URL}`);
  let res = await axios
    .post(
      `${API_URL}/contact/orders/${order.id}/confirm`,
      {},
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function cancelOrder(order) {
  console.log(`Cancelling order ${order.id} ${API_URL}`);
  let res = await axios
    .post(
      `${API_URL}/contact/orders/${order.id}/cancel`,
      {},
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

export async function getOrderStatus(id) {
  console.log(`Fetching order ${id} status from ${API_URL}`);
  let res = await axios
    .get(`${API_URL}/contact/orders/${id}/status`, {
      params: {},
    })
    .then((response) => {
      return response;
    });
  return res.data;
}

export async function payInvoice(invoice) {
  console.log(`Paying for invoice ${invoice.id} ${API_URL}`);
  let res = await axios
    .post(
      `${API_URL}/accounting/invoices/${invoice.id}/pay`,
      {},
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    )
    .then((response) => {
      return response;
    });
  return res;
}

const api = {
  getBooks,
  getBook,
  getBooksPrices,
  getBooksDiscounts,
  getShippingMethods,
  createOrder,
  confirmOrder,
  cancelOrder,
  getOrderStatus,
  payInvoice,
  createBook,
  updateBookInWarehouse,
  updateBookPrice,
  updateBookDiscount,
  createShipping,
  deleteShippingMethod,
  getSensorDataCSV,
  getSensorData
};

export default api;
