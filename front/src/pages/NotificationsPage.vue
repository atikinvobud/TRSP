<script setup lang="ts">
import NotificationBlock from '@/components/NotificationBlock.vue';
import api from '@/services/axios';
import { onMounted, ref } from 'vue'
import type { INotificcationForm } from '@/types/notification';

const notifications = ref<[INotificcationForm]>();
  
  async function fetchNotifications() {
    try {
      const response = await api.get('/notifications/getall')
      notifications.value = response.data
    } catch (error) {
      console.log('Что-то пошло не так во время загрузки уведомлений')
    }
  }

  onMounted(() => {
    fetchNotifications()
  })
</script>

<template>
  <div>
    <p class="text-[32px] font-semibold mt-10">Уведомления</p>
    <div class="grid grid-cols-4 gap-5 mt-10">
    <NotificationBlock
    v-for="notification in notifications"
    :key="notification.id"
    :type="notification.text"
    :lot-name="notification.name"
    :user-name="notification.user"
    :bet="notification.price"
    />
  </div>
  </div>
</template>
