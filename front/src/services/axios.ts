import axios from 'axios'
import router from '@/router/index'

const api = axios.create({
  baseURL: 'http://localhost:5123',
  withCredentials: true,
})

api.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    if (error.response && error.response.status === 401) {
      router.push({ name: 'AuthAndRegPage' })
    }
    return Promise.reject(error)
  },
)

export default api