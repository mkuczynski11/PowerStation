<template>
  <b-container>
    <b-card>
      <b-row>
        <b-col cols="2"></b-col>
        <b-col cols="8">
          <b-button-group>
            <b-button
              v-for="sensorType in SENSOR_TYPES"
              :key="sensorType.name"
              :variant="
                sensorType.name == currentSensorType.name ? 'info' : 'light'
              "
              @click="changeActiveSensorType(sensorType)"
            >
              {{ sensorType.label }}
            </b-button>
          </b-button-group>
        </b-col>
        <b-col cols="2"></b-col>
      </b-row>
      <b-row class="mt-4">
        <b-col cols="2"></b-col>
        <b-col cols="8">
          <b-form-group
            label="Filter"
            label-for="filter-input"
            label-cols-sm="3"
            label-align-sm="right"
            label-size="sm"
            class="mb-0"
          >
            <b-input-group size="sm">
              <b-form-input
                id="filter-input"
                v-model="filter"
                type="search"
                placeholder="Type to Search"
              ></b-form-input>

              <b-input-group-append>
                <b-button :disabled="!filter" @click="filter = ''"
                  >Clear</b-button
                >
              </b-input-group-append>
            </b-input-group>
          </b-form-group>
        </b-col>
        <b-col cols="2"></b-col>
      </b-row>
      <b-row class="mt-4">
        <b-col cols="4"></b-col>
        <b-col cols="4">
          <h6>Filter on:</h6>
          <b-button-group>
            <b-button
              v-for="filter in filters"
              :key="filter.key"
              :variant="filter.active ? 'success' : 'danger'"
              @click="toggleFilter(filter.key)"
            >
              {{ filter.key }}
            </b-button>
          </b-button-group>
        </b-col>
        <b-col cols="4"></b-col>
      </b-row>
      <b-row class="mt-4">
        <b-col cols="1"></b-col>
        <b-col cols="5">
          <b-form-group
            label="Per page"
            label-for="per-page-select"
            label-cols-sm="6"
            label-cols-md="4"
            label-cols-lg="3"
            label-align-sm="right"
            label-size="sm"
            class="mb-0"
          >
            <b-form-select
              id="per-page-select"
              v-model="perPage"
              :options="pageOptions"
              size="sm"
            ></b-form-select>
          </b-form-group>
        </b-col>
        <b-col cols="5">
          <b-pagination
            v-model="currentPage"
            :total-rows="totalRows"
            :per-page="perPage"
            align="fill"
            size="sm"
            class="my-0"
          ></b-pagination>
        </b-col>
        <b-col cols="1"></b-col>
      </b-row>

      <b-row v-if="sensorsLoaded" class="py-4">
        <b-col cols="1"></b-col>
        <b-col>
          <b-table
            cols="10"
            v-if="sensorsLoaded"
            :fields="fields"
            :items="itemsToDisplay"
            :filter="filter"
            :filter-included-fields="chosenFilters"
            :current-page="currentPage"
            :per-page="perPage"
            show-empty
            @filtered="onFiltered"
          >
            <template #cell(value)="data">
              <b>{{ data.value }}</b>
            </template>
            <template #empty="">
              <h4><b>No sensor data found</b></h4>
            </template>
          </b-table>
          <b-row>
            <b-col cols="2"></b-col>
            <b-col cols="3">
              <b-button-group>
                <b-button variant="success" @click="downloadJSON()">
                  JSON
                </b-button>
                <b-button variant="warning" @click="downloadCSV()">
                  CSV
                </b-button>
              </b-button-group>
            </b-col>
            <b-col cols="1">
              <b-button @click="openPlotModal(itemsAfterFiltering)">
                Plot
              </b-button>
            </b-col>
            <b-col cols="6"></b-col>
          </b-row>
        </b-col>
        <b-col cols="1"></b-col>
      </b-row>

      <b-row v-else>
        <b-col cols="12" class="py-4">
          <b-spinner style="width: 3rem; height: 3rem" />
        </b-col>
      </b-row>
    </b-card>
    <b-modal id="plot-modal" size="xl" :hide-header="true">
      <b-row>
        <b-col cols="2"></b-col>
        <b-col cols="8" class="justify-content-center">
          <h4>{{ this.currentSensorType.label}} sensors chart [{{ SENSOR_UNITS[this.currentSensorType.name]}}]</h4>
        </b-col>
        <b-col cols="2"></b-col>
      </b-row>
      <b-container>
        <LineChart
          :chart-options="chartOptions"
          :chart-data="chartData"
          chart-id="line-chart"
          dataset-id-key="label"
        />
      </b-container>
      <template #modal-footer="{}">
        <b-button variant="danger" @click="closePlotModal()"> Cancel </b-button>
      </template>
    </b-modal>
  </b-container>
</template>

<script>
import { getSensorData } from "@/api/api.js";
import { Line as LineChart } from "vue-chartjs/legacy";

import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  LineElement,
  LinearScale,
  CategoryScale,
  PointElement,
} from "chart.js";

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  LineElement,
  LinearScale,
  CategoryScale,
  PointElement
);

const SENSOR_TYPES = [
  { name: "coreTemperature", label: "Core temperature", unit: "°C" },
  { name: "powerGenerated", label: "Power generated", unit: "MWh" },
  { name: "waterUsage", label: "Water usage", unit: "L" },
  { name: "turbineRpm", label: "Turbine RPM", unit: "rpm" },
];

const SENSOR_UNITS = {
  coreTemperature: "°C",
  powerGenerated: "MWh",
  waterUsage: "L",
  turbineRpm: "rpm",
};

const SENSOR_FIELDS = [
  { key: "sensorID", sortable: true },
  { key: "time", sortable: true },
  { key: "value", sortable: true },
];

const FILTERS = [
  { key: "sensorID", active: false },
  { key: "time", active: false },
  { key: "value", active: false },
];

export default {
  name: "Sensors",
  components: {
    LineChart,
  },
  data: function () {
    return {
      sensors: [],
      items: [],
      SENSOR_TYPES: SENSOR_TYPES,
      SENSOR_UNITS: SENSOR_UNITS,
      currentSensorType: SENSOR_TYPES[0],
      itemsToDisplay: [],
      itemsAfterFiltering: [],
      fields: SENSOR_FIELDS,
      filter: "",
      filters: FILTERS,
      chosenFilters: [],
      totalRows: 1,
      currentPage: 1,
      perPage: 5,
      pageOptions: [5, 10, 25, 50, 100, 250],
      sensorsLoaded: false,
      chartData: {
        labels: [],
        datasets: [],
      },
      chartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        spanGaps: true,
      },
      dataToPlot: null,
      order: null,
    };
  },

  async mounted() {
    await this.parseSensors();
  },

  methods: {
    async loadSensors() {
      let res = await getSensorData({ sortBy: "timestamp", sort: "asc" });
      this.sensors = res;
      console.log("Sensors data loaded");
    },

    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
      this.itemsAfterFiltering = filteredItems;
      console.log(this.itemsAfterFiltering);
    },

    downloadJSON() {
      let content = JSON.stringify(this.itemsToDisplay);
      let blob = new Blob([content], { type: "application/json" });

      var url = URL.createObjectURL(blob);
      var pom = document.createElement("a");
      pom.href = url;
      pom.setAttribute("download", "data.json");
      pom.click();
    },

    downloadCSV() {
      const CONTENT = [
        ["sensorID", "sensorType", "timestamp", "time", "value"],
        ...this.itemsToDisplay.map((item) => [
          item.sensorID,
          item.sensorType,
          item.timestamp,
          item.time,
          item.value,
        ]),
      ]
        .map((e) => e.join(","))
        .join("\n");

      let blob = new Blob([CONTENT], { type: "text/csv" });

      var url = URL.createObjectURL(blob);
      var pom = document.createElement("a");
      pom.href = url;
      pom.setAttribute("download", "data.csv");
      pom.click();
    },

    timestampToDate(timestamp) {
      let date = new Date(timestamp);
      return date.toLocaleString();
    },

    initializeSensorItems() {
      this.items = {};

      for (let s of SENSOR_TYPES) {
        this.items[s.name] = [];
      }
    },

    changeActiveSensorType(sensorType) {
      this.currentSensorType = sensorType;

      this.itemsToDisplay = this.items[this.currentSensorType.name];
      this.itemsAfterFiltering = this.itemsToDisplay;
      this.totalRows = this.itemsToDisplay.length;
    },

    toggleFilter(key) {
      let filter = this.filters.find((f) => f.key == key);

      filter.active = !filter.active;

      if (this.chosenFilters.includes(key)) {
        this.chosenFilters.splice(this.chosenFilters.indexOf(key), 1);
      } else {
        this.chosenFilters.push(key);
      }
    },

    async parseSensors() {
      this.sensorsLoaded = false;
      await this.loadSensors();

      this.initializeSensorItems();

      for (let sensor of this.sensors) {
        let newSensorReading = JSON.parse(JSON.stringify(sensor));
        newSensorReading.time = this.timestampToDate(sensor.timestamp);
        newSensorReading.value = `${newSensorReading.value.toFixed(2)} ${
          this.SENSOR_UNITS[newSensorReading.sensorType]
        }`;
        newSensorReading.rawValue = sensor.value;
        this.items[newSensorReading.sensorType].push(newSensorReading);
      }

      this.changeActiveSensorType(this.currentSensorType);

      this.sensorsLoaded = true;
    },

    openPlotModal(dataToPlot) {
      this.dataToPlot = dataToPlot;
      this.dataToPlot = this.dataToPlot.slice(
        (this.currentPage - 1) * this.perPage,
        this.currentPage * this.perPage
      );

      // Prepare plot data
      this.chartData.labels = [];
      this.chartData.datasets = [];

      // Prepare labels
      this.dataToPlot.forEach((sensor) => {
        this.chartData.labels.push({
          time: sensor.time,
          timestamp: sensor.timestamp,
        });
      });
      this.chartData.labels.sort((a, b) => {
        return a.timestamp - b.timesatmp;
      });
      let tmpLabels = [];
      for (let label of this.chartData.labels) {
        if (!tmpLabels.includes(label.time)) {
          tmpLabels.push(label.time);
        }
      }

      this.chartData.labels = tmpLabels;
      this.chartData.labels.sort();

      let coveredSensors = [];
      for (let data of this.dataToPlot) {
        if (!coveredSensors.includes(data.sensorID)) {
          coveredSensors.push(data.sensorID);
        }
      }

      for (let sensor of coveredSensors) {
        this.chartData.datasets.push({
          label: `Sensor ${sensor}`,
          data: [],
          sensorID: sensor,
          backgroundColor:
            "#" + (Math.random().toString(16) + "00000").slice(2, 8),
        });
      }

      for (let data of this.dataToPlot) {
        for (let sensorDataset of this.chartData.datasets) {
          if (sensorDataset.sensorID == data.sensorID) {
            console.log(data);
            sensorDataset.data.push({
              x: data.time,
              y: data.rawValue,
            });
          }
        }
      }

      console.log(this.chartData.datasets);

      this.$bvModal.show("plot-modal");
    },

    closePlotModal() {
      this.$bvModal.hide("plot-modal");
    },
  },
};
</script>

<style lang="scss" scoped></style>
