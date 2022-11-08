<template>
  <b-container>
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
          <template #empty="">
            <h4><b>No sensor data found</b></h4>
          </template>
        </b-table>
        <b-row>
          <b-button-group>
            <b-button variant="success" @click="downloadJSON()">
              JSON
            </b-button>
            <b-button variant="warning" @click="downloadCSV()"> CSV </b-button>
          </b-button-group>
        </b-row>
      </b-col>
      <b-col cols="1"></b-col>
    </b-row>
    <b-row v-else>
      <b-col cols="12" class="py-4">
        <b-spinner style="width: 3rem; height: 3rem" />
      </b-col>
    </b-row>
    <b-modal id="book-reservation-modal" size="lg" :hide-header="true">
      <b-container>
        <h4>Please specify the qunatity</h4>
        <hr />
        <b-row class="pt-2">
          <b-col cols="3">
            <b>Price per book</b>
          </b-col>
          <b-col cols="3" v-if="bookToReserve">
            {{ bookToReserve.unitPrice }} zł
          </b-col>
          <b-col cols="3">
            <b>Total price</b>
          </b-col>
          <b-col cols="3" v-if="bookToReserve && form.quantity">
            {{
              bookToReserve.unitPrice * parseInt(form.quantity) +
              (chosenShippingMethod != null ? chosenShippingMethod.price : 0)
            }}
            zł
          </b-col>
        </b-row>
        <b-row class="pt-2">
          <b-col cols="3">
            <b>Available discount</b>
          </b-col>
          <b-col cols="3" v-if="bookToReserve">
            {{ bookToReserve.discount }} %
          </b-col>
          <b-col cols="3">
            <b>Price after discount</b>
          </b-col>
          <b-col cols="3" v-if="bookToReserve">
            {{
              (
                bookToReserve.unitPrice *
                  parseInt(form.quantity) *
                  (1 - bookToReserve.discount / 100) +
                (chosenShippingMethod != null ? chosenShippingMethod.price : 0)
              ).toFixed(2)
            }}
            zł
          </b-col>
        </b-row>

        <b-row class="pt-2">
          <b-col cols="3">
            <b>Quantity</b>
          </b-col>
          <b-col cols="6">
            <b-form-input
              v-model="form.quantity"
              :type="'number'"
              placeholder="1"
            />
          </b-col>
        </b-row>
        <b-row class="pt-2">
          <b-col cols="3">
            <b>Shipping</b>
          </b-col>
          <b-col cols="6">
            <b-form-select
              v-model="chosenShippingMethod"
              :options="shippingMethods"
            />
          </b-col>
        </b-row>
      </b-container>
      <template #modal-footer="{}">
        <b-button variant="danger" @click="closeBookReservationForm()">
          Cancel
        </b-button>
        <b-button
          variant="success"
          @click="beginOrder()"
          :disabled="!validForm()"
          >Add to the basket!</b-button
        >
      </template>
    </b-modal>
  </b-container>
</template>

<script>
import {
  createOrder,
  getShippingMethods,
  getBooksPrices,
  getBooksDiscounts,
  getSensorData,
} from "@/api/api.js";

const SENSOR_TYPES = [
  { name: "core-temperature", label: "Core temperature", unit: "°C" },
  { name: "power-generated", label: "Power generated", unit: "MWh" },
  { name: "water-usage", label: "Water usage", unit: "L" },
  { name: "turbine-rpm", label: "Turbine RPM", unit: "rpm" },
];

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
  data: function () {
    return {
      sensors: [],
      items: [],
      SENSOR_TYPES: SENSOR_TYPES,
      currentSensorType: SENSOR_TYPES[0],
      itemsToDisplay: [],
      fields: SENSOR_FIELDS,
      filter: "",
      filters: FILTERS,
      chosenFilters: [],
      totalRows: 1,
      currentPage: 1,
      perPage: 5,
      pageOptions: [5, 10, 15, 50, 100],
      sensorsLoaded: false,
      form: {
        quantity: null,
      },
      bookToReserve: null,
      order: null,
      shippingMethods: null,
      chosenShippingMethod: null,
    };
  },

  async mounted() {
    await this.parseSensors();
  },

  methods: {
    async loadSensors() {
      let res = await getSensorData();
      this.sensors = res;
    },

    async loadPrices() {
      let res = await getBooksPrices();

      return res;
    },

    async loadDiscounts() {
      let res = await getBooksDiscounts();

      return res;
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
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
      let date = new Date(timestamp * 1000);
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
        this.items[newSensorReading.sensorType].push(newSensorReading);
      }

      this.changeActiveSensorType(this.currentSensorType);

      this.sensorsLoaded = true;
    },

    async parseShippingMethods() {
      let methods = await getShippingMethods();

      this.shippingMethods = [
        {
          value: null,
          text: "Choose shipping method",
        },
      ];

      for (let method of methods) {
        let newMethod = JSON.parse(JSON.stringify(method));
        this.shippingMethods.push({
          value: {
            method: newMethod.methodValue,
            price: newMethod.price,
          },
          text: newMethod.methodValue,
        });
      }
    },

    openBookReservationForm(book) {
      this.bookToReserve = book;
      this.form.quantity = 1;
      this.chosenShippingMethod = null;

      this.$bvModal.show("book-reservation-modal");
    },

    closeBookReservationForm() {
      this.$bvModal.hide("book-reservation-modal");
    },

    validForm() {
      if (
        this.form["quantity"] <= 0 ||
        this.form["quantity"] > this.bookToReserve.quantity ||
        this.chosenShippingMethod == null
      ) {
        return false;
      }
      return true;
    },

    async beginOrder() {
      this.order = {
        book: this.bookToReserve,
        quantity: parseInt(this.form.quantity), // this quantity must be taken into account, as quantity in book is in regards to total amount
        deliveryMethod: this.chosenShippingMethod,
      };

      let res = await createOrder(this.order);

      if (res.status === 200) {
        this.order.orderId = res.data.orderID;
        this.$orders.unshift(this.order);
        this.$bvToast.toast(`Successfully placed order!`, {
          title: "Order confirmation",
          autoHideDelay: 5000,
          appendToast: true,
        });
      } else {
        this.$bvToast.toast(`Failed to place order!`, {
          title: "Order confirmation",
          autoHideDelay: 5000,
          appendToast: true,
        });
      }

      this.closeBookReservationForm();
    },
  },
};
</script>

<style lang="scss" scoped></style>
