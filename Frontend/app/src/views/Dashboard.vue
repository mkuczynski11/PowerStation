<template>
  <b-container>
    <b-row>
      <b-col cols="2"></b-col>
      <b-col cols="8" class="justify-content-center">Refresh rate</b-col>
      <b-col cols="2"></b-col>
    </b-row>
    <b-row class="m-2">
      <b-col cols="2"></b-col>
      <b-col cols="8">
        <b-button-group>
          <b-button
            v-for="refresh in refreshDelayOptions"
            :key="refresh.value"
            :variant="refresh.active ? 'success' : 'danger'"
            @click="changeRefresh(refresh.value)"
          >
            {{ refresh.value }}
          </b-button>
          <b-button @click="handleInterval()" variant="info">
            Refresh
          </b-button>
          <b-button @click="stopInterval()" variant="warning"> Stop </b-button>
        </b-button-group>
        <b-card class="mt-4">
          <div
            v-for="sensorType in computedValues"
            :key="sensorType.updateId + sensorType.label"
          >
            <b-row>
              <b-col cols="4"></b-col>
              <b-col cols="4"
                ><b>{{ sensorType.label }}</b></b-col
              >
              <b-col cols="4"></b-col>
            </b-row>
            <br />
            <b-row>
              <b-col cols="4"><b>Sensor ID</b></b-col>
              <b-col cols="4"><b>Last value</b></b-col>
              <b-col cols="4"><b>Average value</b></b-col>
            </b-row>
            <b-row
              v-for="sensor in sensorType.sensors"
              :key="sensor.id + sensor.average + sensor.last"
            >
              <b-col cols="4">Sensor {{ sensor.id }}</b-col>
              <b-col cols="4"> {{ sensor.last.toFixed(2) }}</b-col>
              <b-col cols="4"> {{ sensor.average.toFixed(2) }}</b-col>
              <b-col cols="4"></b-col>
            </b-row>
            <hr />
          </div>
          <b-row class="justify-content-center"></b-row>
        </b-card>
      </b-col>
      <b-col cols="2"></b-col>
    </b-row>
  </b-container>
</template>

<script>
import { getSensorData } from "@/api/api.js";

const SENSOR_TYPES = [
  { name: "coreTemperature", label: "Core temperature", unit: "°C" },
  { name: "powerGenerated", label: "Power generated", unit: "MWh" },
  { name: "turbineRpm", label: "Turbine RPM", unit: "rpm" },
  { name: "waterUsage", label: "Water usage", unit: "L" },
];

let refreshDelayOptions = [
  { value: 1, active: false },
  { value: 3, active: false },
  { value: 5, active: true },
  { value: 10, active: false },
  { value: 20, active: false },
];

export default {
  name: "Dashboard",
  data: function () {
    return {
      SENSOR_TYPES: SENSOR_TYPES,
      currentRefreshDelay: 5,
      refreshDelayOptions: refreshDelayOptions,
      lastTimestamp: null,
      sensors: null,
      computedValues: null,
      loadingInterval: null,
      updateId: 0,
    };
  },
  watch: {
    updateId: function(newValue, oldValue) {
      this.computedValues = JSON.parse(JSON.stringify(this.computedValues));
    }
  },

  async mounted() {
    await this.loadNewSensorData(1);
    await this.loadNewSensorData(1);
    this.handleInterval();
    this.changeRefresh(5);
  },

  methods: {
    handleInterval() {
      this.updateId = 1;
      clearInterval(this.loadingInterval);
      console.log("Setting up new interval");

      this.loadingInterval = setInterval(
        () => this.loadNewSensorData(this.lastTimestamp+1),
        this.currentRefreshDelay * 1000
      );
    },

    stopInterval() {
      clearInterval(this.loadingInterval);
      this.$bvToast.toast(`Live refresh stopped!`, {
        title: "Live refresh",
        autoHideDelay: 5000,
        appendToast: true,
      });
    },

    changeRefresh(newRefreshRate) {
      if (newRefreshRate == this.currentRefreshDelay) return;

      let chosenRate = this.refreshDelayOptions.find(
        (f) => f.value == newRefreshRate
      );

      for (let rate of this.refreshDelayOptions) {
        if (rate.value != chosenRate.value) {
          rate.active = false;
        }
      }
      this.currentRefreshDelay = chosenRate.value;
      chosenRate.active = true;

      this.handleInterval();
    },

    initializeSensorItems() {
      this.sensors = {};

      for (let s of SENSOR_TYPES) {
        this.sensors[s.name] = [];
      }
    },

    async loadSensors(lastTimestamp) {
      let params = { sortBy: "timestamp", sort: "asc" };

      if (lastTimestamp) {
        params.timestampFrom = lastTimestamp;
      }

      let res = await getSensorData(params);

      if (res.length > 0) {
        this.lastTimestamp = res.slice(-1)[0].timestamp;
      }
      return res;
    },

    // This function will be called inside an interval
    async loadNewSensorData(lastTimestamp) {
      let newSensorReadings = await this.loadSensors(lastTimestamp);
      this.updateId++;

      if (!this.computedValues) {
        this.computedValues = {};

        let id = 1;
        for (let s of SENSOR_TYPES) {
          this.computedValues[s.name] = {};
          this.computedValues[s.name].sensors = {};
          this.computedValues[s.name].label = s.label;
          this.computedValues[s.name].name = s.name;
          this.computedValues[s.name].updateId = this.updateId;
          for (let i = id; i < id + 8; i++) {
            this.computedValues[s.name].sensors[i] = {
              id: i,
              last: 0,
              average: 0,
              data: [],
            };
          }
          id += 8;
        }
      }

      console.log(
        `Loaded ${newSensorReadings.length} new readings | updateID = ${this.updateId} | ${this.lastTimestamp}`
      );

      // Divide new sensors for given sensor type
      for (let sensor of newSensorReadings) {
        this.computedValues[sensor.sensorType].sensors[
          sensor.sensorID
        ].data.push({
          timestamp: sensor.timestamp,
          value: sensor.value,
        });
      }

      // Trim the array if length > 100
      // Check arrays lengths
      for (let sensorType in this.computedValues) {
        // console.log(sensorType);
        this.computedValues[sensorType].updateId = this.updateId;
        for (let sensorId in this.computedValues[sensorType].sensors) {
          // console.log(this.computedValues[sensorType].sensors[sensorId]);
          let currentReadingsAmount =
            this.computedValues[sensorType].sensors[sensorId].data.length;
          if (currentReadingsAmount > 100) {
            this.computedValues[sensorType].sensors[sensorId].data =
              this.computedValues[sensorType].sensors[sensorId].data.splice(
                0,
                currentReadingsAmount - 100
              );
          }
        }
      }

      // Compute average and set the last value
      for (let sensorType in this.computedValues) {
        for (let sensorId in this.computedValues[sensorType].sensors) {
          let readingAmount =
            this.computedValues[sensorType].sensors[sensorId].data.length;
          if (readingAmount > 0) {
            this.computedValues[sensorType].sensors[sensorId].last =
              this.computedValues[sensorType].sensors[sensorId].data[
                readingAmount - 1
              ].value;
            this.computedValues[sensorType].sensors[sensorId].average =
              this.computedValues[sensorType].sensors[sensorId].data.reduce(
                (a, b) => a + b.value,
                0
              ) / this.computedValues[sensorType].sensors[sensorId].data.length;
          }
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped></style>
