import { ref } from 'vue'
import mitt from 'mitt'

type Events = {
  open: void
  message: string
  error: Event
}

const emitter = mitt<Events>()
const socket = ref<WebSocket | null>(null)

export async function createSocket() {
  if (!socket.value || socket.value.readyState === WebSocket.CLOSED) {
    socket.value = new WebSocket('ws://localhost:5123/ws')

    socket.value.addEventListener('open', () => {
      emitter.emit('open')
    })

    socket.value.addEventListener('message', (evt) => {
      emitter.emit('message', evt.data)
    })

    socket.value.addEventListener('error', (err) => {
      emitter.emit('error', err)
    })
  }
  return socket.value
}

export function send(data: unknown) {
  if (socket.value && socket.value.readyState === WebSocket.OPEN) {
    socket.value.send(JSON.stringify(data))
  }
}

export function onOpen(fn: () => void): () => void {
  emitter.on('open', fn)
  return () => { emitter.off('open', fn) }
}

export function onMessage(fn: (msg: string) => void): () => void {
  emitter.on('message', fn)
  return () => { emitter.off('message', fn) }
}

export function onError(fn: (e: Event) => void): () => void {
  emitter.on('error', fn)
  return () => { emitter.off('error', fn) }
}