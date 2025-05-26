export interface ILotForm {
  name: string | null
  author: string | null
  year: number | null
  date: string[]
  startPrice: number | null
  image: File | null
}

export interface ILotShortInfo {
  id: number
  name: string
  imageUrl: string
  author: string
  year: number
  seller: string
  amountOfBids: number
  leader: string | null
  startDate: string
  endDate: string
  price: number
}
