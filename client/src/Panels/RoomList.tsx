import { useEffect, useState } from 'react'
import { useQuery } from 'react-query'
import Loading from '../Components/Loading'
import RoomButton from '../Components/RoomButton'
import { useApi } from '../Hooks/ApiProvider'
import { useRoomHub } from '../Hooks/RoomHubProvider'
import RoomData from '../Types/RoomData'
import classes from './RoomList.module.scss'

const RoomList = () => {
    const { connectedRoomId, connectToRoom } = useRoomHub()
    const { apiGet } = useApi()
    const { isLoading, error, data: rooms } = useQuery<RoomData[], Error>("rooms", () => apiGet("rooms/getall"))

    const onRoomButtonClick = (roomId: string) => {
        connectToRoom(roomId)
    }

    return (
        <div className={`d-flex flex-column flex-grow-1 p-1 ${classes.background} ${classes.shadow}`}>
            <Loading isLoading={isLoading && !error} >
                {
                    rooms && !error && rooms.map(room => <RoomButton key={room.id} roomName={room.name} selected={room.id === connectedRoomId} onClick={() => onRoomButtonClick(room.id)} />)
                }
            </Loading>
        </div>
    )
}

export default RoomList