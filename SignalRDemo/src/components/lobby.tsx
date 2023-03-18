import { useState, useEffect } from 'react'
import { Form, Button } from 'react-bootstrap'


interface Props
{
    logInUrl: string
}

export const Lobby = (props: Props) => {

    useEffect(() => {
        console.log('Loaded')
    }, []);

    const [user, setUser] = useState('');

    const logIn = (e: React.FormEvent) =>
    {
        console.log(user)

        e.preventDefault()

        fetch(props.logInUrl, {
            method: 'POST',
            headers: {
              Accept: 'application.json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify({ 'name': user })
          })
    }

    return <Form className='lobby'
        onSubmit={logIn}>
        <Form.Group>
            <Form.Control placeholder="name" onChange={e => {
                console.log(user)
                setUser(e.target.value)} 
            }/>
        </Form.Group>
        <Button variant="success" type="submit">Join</Button>
    </Form>
}