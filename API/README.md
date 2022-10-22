# Power Station app

### Queue configuration
For each sensor type there will be a queue [unit]
- core-temperature [C]
- power-generated [MWh]
- water-usage [l]
- turbine-rpm [rpm]

For each queue there will be a consumer
- coreTemperatureConsumer
- electricityProductConsumer
- UraniumUsageConsumer
- RadioactivityConsumer

### Database
SensorData:
- ID
- SensorID
- SensorType
- Value
- Timestamp

### API endpoint
ONLY GET REQUESTS

"api/data"
"api/data/csv"

#### Query params:
- sensorID
- sensorType=[coreTemperature, electricityProduct, uraniumUsage, radioactivity]
- timestampFrom
- timestampTo
- sortBy=[sensorID, sensorType, timestamp, value]
- sort=[asc, dsc(default)]
