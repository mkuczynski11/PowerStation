<template>
  <b-container>
    <b-row>
      <b-col cols="2"></b-col>
      <b-col cols="8" class="justify-content-center">Refresh rate</b-col>
      <b-col cols="2"></b-col>
    </b-row>
    <b-row v-if="sensorsDataLoaded" class="m-2">
      <b-col cols="2"></b-col>
      <b-col cols="8">
        <b-button-group>
          <b-button
            v-for="refresh in refreshDelayOptions"
            :key="refresh.key"
            :variant="refresh.active ? 'success' : 'danger'"
            @click="changeRefresh(refresh.key)"
          >
            {{ refresh.key }}
          </b-button>
        </b-button-group>
        <b-card class="mt-4">
          <div v-for="sensorType in computedValues" :key="sensorType.label">
            <b-row>
              <b-col cols="4"></b-col>
              <b-col cols="4"
                ><b>{{ sensorType.label }}</b></b-col
              >
              <b-col cols="4"></b-col>
            </b-row>
            <br/>
            <b-row>
              <b-col cols="4"><b>Sensor ID</b></b-col>
              <b-col cols="4"><b>Last value</b></b-col>
              <b-col cols="4"><b>Average value</b></b-col>
            </b-row>
            <b-row v-for="sensor in sensorType.sensors" :key="sensor.id">
              <b-col cols="4">Sensor {{sensor.id}}</b-col>
              <b-col cols="4"> {{ sensor.last }}</b-col>
              <b-col cols="4"> {{ sensor.average }}</b-col>
              <b-col cols="4"></b-col>
            </b-row>
            <hr />
          </div>
          <b-row class="justify-content-center"></b-row>
        </b-card>
      </b-col>
      <b-col cols="2"></b-col>
    </b-row>
    <b-row v-else>
      <b-col cols="12" class="mt-2">
        <b-spinner style="width: 3rem; height: 3rem" />
      </b-col>
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
  { key: 1, active: false },
  { key: 3, active: false },
  { key: 5, active: true },
  { key: 10, active: false },
  { key: 20, active: false },
];

export default {
  name: "Dashboard",
  data: function () {
    return {
      SENSOR_TYPES: SENSOR_TYPES,
      sensorsDataLoaded: false,
      refreshRateChanging: false,
      currentRefreshDelay: 5,
      refreshDelayOptions: refreshDelayOptions,
      lastTimestamp: null,
      sensors: null,
      computedValues: null,
      loadingInterval: null,
    };
  },

  async mounted() {
    this.initializeComputedValues();
    await this.parseSensors();

    // TODO: Handle initiating interval
  },

  methods: {
    handleInterval() {
      if (this.loadingInterval) {
        clearInterval(this.loadingInterval);
      }

      this.loadingInterval = setInterval(
        () => this.loadNewSensorData(this.lastTimestamp),
        this.currentRefreshDelay
      );
    },

    changeRefresh(newRefreshRate) {
      if (newRefreshRate == this.currentRefreshDelay) return;

      let chosenRate = this.refreshDelayOptions.find(
        (f) => f.key == newRefreshRate
      );

      for (let rate of this.refreshDelayOptions) {
        if (rate.key != chosenRate.key) {
          rate.active = false;
        }
      }
      this.currentRefreshDelay = chosenRate.key;
      chosenRate.active = true;

      // TODO: Handle changing interval
    },

    initializeSensorItems() {
      this.sensors = {};

      for (let s of SENSOR_TYPES) {
        this.sensors[s.name] = [];
      }
    },

    initializeComputedValues() {
      this.computedValues = {};

      let id = 1;
      for (let s of SENSOR_TYPES) {
        this.computedValues[s.name] = {};
        this.computedValues[s.name].sensors = [];
        this.computedValues[s.name].label = s.label;
        for (let i = id; i < id + 8; i++) {
          this.computedValues[s.name].sensors.push({
            id: i,
            last: 0,
            average: 0,
            data: [],
          });
        }
        id += 8;
      }
      // computedValues = {
      //  coreTemperature" {
      //    label: "Core temperature",
      //    sensors: [
      //        {id: 1, last: 123, average: 321, data: [...{timestamp: 123, value: 231}]}
      //      ]
      //    }
      // }
    },

    async loadSensors(lastTimestamp) {
      let params = { sortBy: "timestamp" };

      if (lastTimestamp) {
        params.timestampFrom = lastTimestamp;
      }

      let res = await getSensorData(params);

      this.lastTimestamp = res.slice(-1).timestamp;
      console.log("Sensors data loaded");

      return res;
    },

    async parseSensors() {
      this.sensorsDataLoaded = false;
      let rawData = await this.loadSensors();

      this.initializeSensorItems();

      for (let sensor of rawData) {
        this.sensors[sensor.sensorType].push(sensor);
      }

      this.sensorsDataLoaded = true;
    },

    // This function will be called inside an interval
    async loadNewSensorData(lastTimestamp) {
      let newSensorReadings = await this.getSensorData(lastTimestamp);

      // Divide new sensors for given sensor type
      for (let sensor of newSensorReadings) {
        this.computedValues[sensor.sensorType].data.push({
          timestamp: sensor.timestamp,
          value: sensor.value,
        });
      }

      // Computed values needs to be updated
      for (let sensor of this.SENSOR_TYPES) {
        let SENSOR_TYPE = sensor.name;

        let valuesToUpdate = this.computedValues[SENSOR_TYPE];
      }

      // Trim the array if length > 100
    },
  },
};
</script>

<style lang="scss" scoped></style>
