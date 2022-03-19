import axios from 'axios'
import { useState } from 'react'
import { useQuery } from 'react-query'
import Loading from '../Components/Loading'
import RoomButton from '../Components/RoomButton'
import classes from './RoomList.module.scss'

type Room = {
    id: string,
    name: string,
}

const RoomList = () => {
    const { isLoading, error, data: rooms } = useQuery<Room[], Error>("rooms", () => axios.get("https://localhost:7177/api/rooms/getall").then((res) => res.data))

    const [selectedRoom, setSelectedRoom] = useState('')

    if (error) {
        return (
            <div>Error getting rooms</div>
        )
    }

    return (
        <div className={`d-flex flex-column p-1 ${classes.background} ${classes.shadow}`}>
            <Loading isLoading={isLoading} >
                {
                    !rooms ?
                        <div>No rooms found</div> :
                        rooms.map(room => <RoomButton key={room.id} roomName={room.name} selected={room.name === selectedRoom} onClick={() => setSelectedRoom(room.name)} />)
                }
            </Loading>
        </div>
    )
}

export default RoomList