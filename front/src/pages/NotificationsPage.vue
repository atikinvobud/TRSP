<script setup lang="ts">
  import NotificationBlock from '@/components/NotificationBlock.vue'
  import api from '@/services/axios'
  import { onBeforeUnmount, onMounted, ref, watch } from 'vue'
  import { vAutoAnimate } from '@formkit/auto-animate'
  import type { INotificcationForm } from '@/types/notification'
  import { createSocket, send, onOpen, onMessage, onError } from '@/services/socket'

  const notifications = ref<INotificcationForm[]>([])

  async function fetchNotifications() {
    try {
      const response = await api.get('/notifications/getall')
      notifications.value = response.data
    } catch (error) {
      console.log('Что-то пошло не так во время загрузки уведомлений')
    }
  }

  let offOpen: () => void
  let offMsg: () => void
  let offErr: () => void

  onMounted(async () => {
    await fetchNotifications()

    await createSocket()

    offOpen = onOpen(() => {
      console.log('Socket открыт')
      send({ type: 'ping' })
    })
    offMsg = onMessage((data) => {
      console.log('⬅Получили:', data)
      fetchNotifications()
    })
    offErr = onError((err) => {
      console.error('Ошибка сокета', err)
    })
  })

  onBeforeUnmount(() => {
    offOpen()
    offMsg()
    offErr()
  })
</script>

<template>
  <div>
    <p class="text-[32px] font-semibold mt-10">Уведомления</p>
    <div v-auto-animate class="grid grid-cols-4 gap-5 mt-10">
      <NotificationBlock
        v-auto
        v-for="notification in [...notifications].reverse()"
        :key="notification.id"
        :type="notification.text"
        :lot-name="notification.name"
        :user-name="notification.user"
        :bet="notification.price"
      />
    </div>
  </div>
</template>
