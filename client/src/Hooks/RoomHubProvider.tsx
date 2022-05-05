import { useState, useEffect, createContext, useContext } from 'react'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { useAuth0 } from '@auth0/auth0-react'

const getHubUrl = () => {
    if (process.env.NODE_ENV && process.env.NODE_ENV === 'production') {
        return "https://localhost:7177/hub/room"
    }
    return "https://localhost:7177/hub/room"
}

interface RoomHubInterface {
    connection: HubConnection | undefined,
    connectedRoomId: string | undefined,
    connectToRoom: (roomId: string) => void,
    disconnectFromRoom: () => void,
}

const RoomHubContect = createContext<RoomHubInterface>({
    connection: undefined,
    connectedRoomId: undefined,
    connectToRoom: () => console.error("RoomHubProvider not initialized"),
    disconnectFromRoom: () => console.error("RoomHubProvider not initialized")
})

export const RoomHubProvider = (props: React.PropsWithChildren<{}>) => {
    const { getAccessTokenSilently } = useAuth0()

    const [connection, setConnection] = useState<HubConnection | undefined>(undefined)
    const [currentRoom, setCurrentRoom] = useState<string | undefined>(undefined)

    const connectToRoom = (roomId: string) => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(getHubUrl(), { accessTokenFactory: async () => await getAccessTokenSilently() })
            .withAutomaticReconnect()
            .build()
        setConnection(newConnection)
        setCurrentRoom(roomId)
    }

    const disconnectFromRoom = () => {
        if (currentRoom === undefined) return
        if (connection === undefined) return

        setCurrentRoom(undefined)
    }

    useEffect(() => {
        if (currentRoom === undefined) return
        if (connection && connection.state === "Disconnected") {
            connection.start()
                .then(async () => await connection.send('JoinRoom', currentRoom))
                .catch(e => console.log('Failed to connect to queue: ', e))
        }
    }, [connection, currentRoom])

    const value = {
        connection: connection,
        connectedRoomId: currentRoom,
        connectToRoom: connectToRoom,
        disconnectFromRoom: disconnectFromRoom
    }

    return <RoomHubContect.Provider value={value} {...props} />
}

export const useRoomHub = () => useContext(RoomHubContect)