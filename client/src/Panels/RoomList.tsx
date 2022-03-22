import { useState } from 'react'
import { useQuery } from 'react-query'
import Loading from '../Components/Loading'
import RoomButton from '../Components/RoomButton'
import { useApi } from '../Hooks/ApiProvider'
import classes from './RoomList.module.scss'

type Room = {
    id: string,
    name: string,
}

const RoomList = () => {
    const { apiGet } = useApi()

    const { isLoading, error, data: rooms } = useQuery<Room[], Error>("rooms", () => apiGet("rooms/getall"))

    const [selectedRoom, setSelectedRoom] = useState('')

    return (
        <div className={`d-flex flex-column flex-grow-1 p-1 ${classes.background} ${classes.shadow}`}>
            <Loading isLoading={isLoading && !error} >
                {
                    !rooms || error ?
                        <div>No rooms found</div> :
                        rooms.map(room => <RoomButton key={room.id} roomName={room.name} selected={room.name === selectedRoom} onClick={() => setSelectedRoom(room.name)} />)
                }
            </Loading>
        </div>
    )
}

export default RoomList