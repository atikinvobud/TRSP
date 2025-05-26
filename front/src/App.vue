<script setup lang="ts">
  import AppHeader from './components/AppHeader.vue'
</script>

<template>
  <AppHeader></AppHeader>
  <RouterView class="w-[1196px] mx-auto"></RouterView>
</template>

<style>
  body {
    font-family: 'Montserrat', sans-serif;
    font-size: 16px;
    font-weight: 500;
    color: rgba(0, 0, 0, 0.9);
  }
</style>
