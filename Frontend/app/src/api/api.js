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

const api = {
  getSensorDataCSV,
  getSensorData
};

export default api;
