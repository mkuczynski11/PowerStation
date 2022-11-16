<template>
  <div class="home">
    <b-container>
      <b-row> 
        <b-col cols="2"></b-col>
        <b-col cols="8">
          <b-card>
            <h4>Power plant sensors system</h4>
            <hr/>
            <h5>A project for <i>.NET Web Services</i> course prepared by Michał Bałasz, Michał Flakowski, Martin Kuczyński and Piotr Woliński.</h5>
            <h5><i>Sensors</i> tab contains informations about sensors in various ways. Data can be either plot on charts, viewed in tables or downoladed as CSV/JSON file.</h5>
            <h5>In the <i>Dashboard</i> tab one may find live informations about the sensors.</h5>
          </b-card>
        </b-col>
        <b-col cols="2"></b-col>
      </b-row>
    </b-container>
  </div>
</template>

<script>
export default {
  name: "Home",
};
</script>
